﻿using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace EliteAPI.BuildTools
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            while (true)
            {
                counter = 0;

                Console.Write(" > ");
                string[] arg = Console.ReadLine()?.Split(' ');

                if (arg != null)
                {
                    string command = arg[0].ToLower();
                    string argument = string.Join(' ', arg.Skip(1));

                    switch (command)
                    {
                        case "json":
                            string generated = await FromJson(argument);
                            await File.WriteAllTextAsync("generated.cs", generated);
                            break;

                        case "sort":
                            await Sort(argument);
                            break;

                        case "test":
                            await Tests();
                            break;
                    }
                }
                Console.WriteLine();
            }
        }

        private static async Task Tests()
        {


        }

        private static async Task Sort(string path)
        {
            if (Directory.Exists(path))
            {
                foreach (FileInfo file in new DirectoryInfo(path).GetFiles())
                {
                    await Sort(file.FullName);
                }
            }

            if (File.Exists(path))
            {
                string version = Regex.Match(await File.ReadAllTextAsync(path), "\"gameversion\":\"(.*?)\"").Groups[1].Value;

                Directory.CreateDirectory($"Journal/{version}");

                string[] entries = await File.ReadAllLinesAsync(path);
                foreach (string entry in entries)
                {
                    try
                    {
                        string file = $"Journal/{version}/{JsonConvert.DeserializeObject<dynamic>(entry).@event}.json";
                        if (!File.Exists(file) || !(await File.ReadAllLinesAsync(file)).Contains(entry))
                        {
                            await File.AppendAllTextAsync(file, entry + Environment.NewLine);
                        }
                    }
                    catch
                    {

                    }

                }
            }
        }

        private static Task<string> GetEventName(string type)
        {
            return Task.FromResult(Regex.Match(type, "public partial class (.*)\n").Groups[1].Value.Trim());
        }

        private static Task<string> GetEventNameFromJson(string json)
        {
            return Task.FromResult((string)JsonConvert.DeserializeObject<dynamic>(json).@event + "Event");
        }

        private static async Task WriteInputFile(string json)
        {
            // Remove weird " and '
            json = json.Replace('\u2018', '\'').Replace('\u2019', '\'').Replace('\u201c', '\"').Replace('\u201d', '\"');
            
            // Remove event attribute
            json = Regex.Replace(json, "(\"event\":.*?,)", string.Empty);

            // Remove timestamp attribute
            json = Regex.Replace(json, "(\"timestamp\":.*?,)", string.Empty);

            await File.WriteAllTextAsync("input", json);
        }

        private static async Task<string> ConvertToQuickType(string json)
        {
            await WriteInputFile(json);

            string eventName = await GetEventNameFromJson(json);

            Process proc = new Process
            {
                StartInfo = new ProcessStartInfo()
                {
                    FileName = "cmd",
                    Arguments = $"/c quicktype --lang cs input --namespace EliteAPI.Event.Models --array-type list --features complete --no-maps --top-level {eventName}",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                    WorkingDirectory = Directory.GetCurrentDirectory()
                }
            };

            proc.Start();

            return await proc.StandardOutput.ReadToEndAsync();
        }

        private static async Task<string> FromJson(string json)
        {
            try
            {
                string type = await ConvertToQuickType(json);
                string eventName = await GetEventName(type);

                // Remove all comments
                type = Regex.Replace(type, "//.*\n", string.Empty);

                // Remove second parameter in FromJson method
                type = Regex.Replace(type, "json, EliteAPI.Event.Models.Converter.Settings", "json");

                // Remove Serialize and Converter classes
                type = Regex.Replace(type, "public static class Serialize.*\\}", "\n}", RegexOptions.Singleline);

                // Add internal constructor to main class
                type = Regex.Replace(type, "public partial class (.*)\n    {", "$&\n        internal $1() { }\n");

                // Remove constructor on second half of partial main class
                type = Regex.Replace(type, $"internal {eventName}.*\n.*\n.*(public static)", "$1");

                // Add EventBase inheritance
                type = Regex.Replace(type, $"using.*?public partial class {eventName}", "$0 : EventBase", RegexOptions.Singleline);

                // Add using Abstractions statement
                type = Regex.Replace(type, "using Newtonsoft.Json.Converters;\n", "$0    using Abstractions;\n\n");

                // Change all setters to private setters
                type = Regex.Replace(type, "set;", "private set;");

                // Change all lists to readonly lists
                type = Regex.Replace(type, "List", "IReadOnlyList");

                // Check if we have any subclasses
                if (Regex.Matches(type, "public partial class.*?public partial class.*?", RegexOptions.Singleline).Count > 1)
                {
                    // Rename *Class types to just *
                    type = Regex.Replace(type, " (.*?)Class", " $1");

                    // Remove closing tag in main class, making all subclasses actual subclasses
                    type = Regex.Replace(type, "(EventBase\n.*?}\n    )}", "$1", RegexOptions.Singleline);
                    
                    // idk
                    type = Regex.Replace(type, $"}}\n\n    public partial class {eventName}", "}\n\n$&", RegexOptions.Singleline);

                    // Get all subclass names
                    IEnumerable<string> subClasses = Regex.Matches(type, "public partial class (.*?)\n").ToList().Select(x => x.Groups[1].Value);
                    foreach (string subClass in subClasses)
                    {
                        if (subClass.Trim() == eventName || subClass.Trim() == "EventBase") { continue; }

                        Console.WriteLine(subClass);

                        // Rename subclasses to *Info
                        type = Regex.Replace(type, $"{subClass}", $"{subClass.Trim()}Info");

                        // Rename properties back from *Info to just *
                        type = Regex.Replace(type, $"{subClass}Info {{ get", $"{subClass.Trim()} {{ get");
                    }

                    // Remove accidental double Info's
                    type = Regex.Replace(type, "InfoInfo", "Info");

                    // Make sure EventBaseInfo does not exist
                    type = Regex.Replace(type, "EventBaseInfo", "EventBase");
                }

                StringBuilder eventHandler = new StringBuilder();
                eventHandler.AppendLine();
                eventHandler.AppendLine("namespace EliteAPI.Event.Handler");
                eventHandler.AppendLine("{");
                eventHandler.AppendLine("    using System;");
                eventHandler.AppendLine("    using Models;");
                eventHandler.AppendLine();
                eventHandler.AppendLine("    public partial class EventHandler");
                eventHandler.AppendLine("    {");
                eventHandler.AppendLine($"        public event EventHandler<{eventName}> {eventName};");
                eventHandler.AppendLine($"        internal void Invoke{eventName}({eventName} arg) => {eventName}?.Invoke(this, arg);");
                eventHandler.AppendLine("    }");
                eventHandler.AppendLine("}");

                type += eventHandler;

                return type;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return "";
            }
        }

        private static IEnumerable<string> ReadAllLines(FileInfo file)
        {
            using FileStream fs = new FileStream(file.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, 0x1000, FileOptions.RandomAccess);
            using StreamReader stream = new StreamReader(fs, Encoding.UTF8);

            string line;
            while ((line = stream.ReadLine()) != null)
            {
                yield return line;
            }
        }

        private static string ReadAllText(FileInfo file)
        {
            return string.Join(Environment.NewLine, ReadAllLines(file));
        }

        private static int counter = 0;
        private static void Debug(string json)
        {
            string text = $"#{counter}\n{json}\n\n";

            if (counter == 0)
            {
                File.WriteAllText("debug.txt", text);
            }
            else
            {
                File.AppendAllText("debug.txt", text);
            }

            counter++;
        }
    }
}