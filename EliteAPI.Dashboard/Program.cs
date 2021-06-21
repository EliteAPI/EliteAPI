using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronNET.API;
using EliteAPI.Abstractions;
using EliteAPI.Journal.Processor.Abstractions;
using EliteAPI.Status.Processor.Abstractions;
using EliteAPI.Status.Provider.Abstractions;
using EliteAPI_app.WebSockets.Handler;
using EliteAPI_app.WebSockets.Logging;
using EliteAPI_app.WebSockets.Message;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Valsom.Logging.PrettyConsole;
using Valsom.Logging.PrettyConsole.Formats;
using Valsom.Logging.PrettyConsole.Themes;

namespace EliteAPI_app
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            
            var api = host.Services.GetRequiredService<IEliteDangerousApi>();
            var socketHandler = host.Services.GetRequiredService<WebSocketHandler>();
            
            await api.InitializeAsync();
            
            // Sub to events
            api.Events.AllEvent += async (sender, e) => await socketHandler.Broadcast(new WebSocketMessage("Event", e.ToJson()), true);
            
            // Sub to statuses
            api.Cargo.OnChange += async (sender, e) => await socketHandler.Broadcast(new WebSocketMessage("Cargo", api.Cargo.ToJson()), true, true);
            api.Market.OnChange += async (sender, e) => await socketHandler.Broadcast(new WebSocketMessage("Market", api.Market.ToJson()), true, true);
            api.Modules.OnChange += async (sender, e) => await socketHandler.Broadcast(new WebSocketMessage("Modules", api.Modules.ToJson()), true, true);
            api.Outfitting.OnChange += async (sender, e) => await socketHandler.Broadcast(new WebSocketMessage("Outfitting", api.Outfitting.ToJson()), true, true);
            api.Shipyard.OnChange += async (sender, e) => await socketHandler.Broadcast(new WebSocketMessage("Shipyard", api.Shipyard.ToJson()), true, true);
            api.Ship.OnChange += async (sender, e) => await socketHandler.Broadcast(new WebSocketMessage("Status", api.Ship.ToJson()), true, true);
            api.NavRoute.OnChange += async (sender, e) => await socketHandler.Broadcast(new WebSocketMessage("NavRoute", api.NavRoute.ToJson()), true, true);
            api.Bindings.OnChange += async (sender, e) => await socketHandler.Broadcast(new WebSocketMessage("Bindings", api.Bindings.ToJson()), true, true);

            await api.StartAsync();

            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging(logger =>
                {
                    logger.ClearProviders();
                    logger.SetMinimumLevel(LogLevel.Trace);
                    //logger.AddPrettyConsole(ConsoleFormats.Default, ConsoleThemes.Vanilla);
                    logger.AddSimpleConsole();
                    logger.AddWebSocketLogger();
                })
                .ConfigureAppConfiguration(config => { config.AddIniFile("EliteAPI.ini", true, true); })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseElectron(args);
                    webBuilder.UseStartup<Startup>();
                });
    }
}