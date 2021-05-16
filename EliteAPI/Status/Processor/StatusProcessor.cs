﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using EliteAPI.Services.FileReader.Abstractions;
using EliteAPI.Status.Abstractions;
using EliteAPI.Status.Cargo.Abstractions;
using EliteAPI.Status.Cargo.Raw;
using EliteAPI.Status.Commander.Abstractions;
using EliteAPI.Status.Market.Abstractions;
using EliteAPI.Status.Market.Raw;
using EliteAPI.Status.Modules.Abstractions;
using EliteAPI.Status.Modules.Raw;
using EliteAPI.Status.NavRoute.Abstractions;
using EliteAPI.Status.NavRoute.Raw;
using EliteAPI.Status.Outfitting.Abstractions;
using EliteAPI.Status.Outfitting.Raw;
using EliteAPI.Status.Processor.Abstractions;
using EliteAPI.Status.Raw;
using EliteAPI.Status.Ship.Abstractions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace EliteAPI.Status.Processor
{
    /// <inheritdoc />
    public class StatusProcessor : IStatusProcessor
    {
        private readonly IDictionary<string, string> _cache;
        private readonly ICargo _cargo;
        private readonly ILogger<StatusProcessor> _log;
        private readonly IMarket _market;
        private readonly IModules _modules;
        private readonly INavRoute _navRoute;
        private readonly IOutfitting _outfitting;
        private readonly IFileReader _fileReader;

        private readonly IShip _ship;
        private readonly ICommander _commander;

        public StatusProcessor(ILogger<StatusProcessor> log, IShip ship, ICommander commander, INavRoute navRoute,
            ICargo cargo, IMarket market, IModules modules, IOutfitting outfitting, IFileReader fileReader)
        {
            _log = log;
            _ship = ship;
            _commander = commander;
            _navRoute = navRoute;
            _cargo = cargo;
            _market = market;
            _modules = modules;
            _outfitting = outfitting;
            _fileReader = fileReader;
            _cache = new Dictionary<string, string>();
        }

        /// <inheritdoc />
        public event EventHandler<(string Json, RawStatus Ship)> StatusUpdated;

        /// <inheritdoc />
        [Obsolete("Use StatusUpdated instead", true)]
        public event EventHandler<(string Json, RawStatus Ship)> ShipUpdated;

        /// <inheritdoc />
        public event EventHandler<(string Json, RawModules Modules)> ModulesUpdated;

        /// <inheritdoc />
        public event EventHandler<(string Json, RawCargo Cargo)> CargoUpdated;

        /// <inheritdoc />
        public event EventHandler<(string Json, RawMarket Market)> MarketUpdated;

        /// <inheritdoc />
        [Obsolete("Not yet implemented")]
        public event EventHandler<string> ShipyardUpdated;
        //public event EventHandler<(string Json, RawShipyard Shipyard)> ShipyardUpdated;

        /// <inheritdoc />
        public event EventHandler<(string Json, RawOutfitting Outfitting)> OutfittingUpdated;

        /// <inheritdoc />
        public event EventHandler<(string Json, RawNavRoute NavRoute)> NavRouteUpdated;

        /// <inheritdoc />
        public async Task ProcessStatusFile(FileInfo statusFile)
        {
            if (statusFile == null || !statusFile.Exists) return;

            var content = _fileReader.ReadAllText(statusFile);

            if (string.IsNullOrWhiteSpace(content))
            {
                _log.LogTrace("Skipping status processing due to empty Status.json file");
                return;
            }

            if (!IsInCache(statusFile, content))
            {
                AddToCache(statusFile, content);
                var raw = JsonConvert.DeserializeObject<RawStatus>(content);

                UpdateStatusProperty(_ship.Flags, raw.ShipFlags, "Ship.Flags");
                UpdateStatusProperty(_ship.Docked, raw.Docked, "Ship.Docked");
                UpdateStatusProperty(_ship.Landed, raw.Landed, "Ship.Landed");
                UpdateStatusProperty(_ship.Gear, raw.Gear, "Ship.Gear");
                UpdateStatusProperty(_ship.Shields, raw.Shields, "Ship.Shields");
                UpdateStatusProperty(_ship.Supercruise, raw.Supercruise, "Ship.Supercruise");
                UpdateStatusProperty(_ship.FlightAssist, raw.FlightAssist, "Ship.FlightAssist");
                UpdateStatusProperty(_ship.Hardpoints, raw.Hardpoints, "Ship.Hardpoints");
                UpdateStatusProperty(_ship.Winging, raw.Winging, "Ship.Winging");
                UpdateStatusProperty(_ship.Lights, raw.Lights, "Ship.Lights");
                UpdateStatusProperty(_ship.CargoScoop, raw.CargoScoop, "Ship.CargoScoop");
                UpdateStatusProperty(_ship.SrvHandbreak, raw.SrvHandbreak, "Ship.SrvHandbreak");
                UpdateStatusProperty(_ship.SrvTurrent, raw.SrvTurrent, "Ship.SrvTurrent");
                UpdateStatusProperty(_ship.SrvNearShip, raw.SrvNearShip, "Ship.SrvNearShip");
                UpdateStatusProperty(_ship.SrvDriveAssist, raw.SrvDriveAssist, "Ship.SrvDriveAssist");
                UpdateStatusProperty(_ship.MassLocked, raw.MassLocked, "Ship.MassLocked");
                UpdateStatusProperty(_ship.FsdCharging, raw.FsdCharging, "Ship.FsdCharging");
                UpdateStatusProperty(_ship.FsdCooldown, raw.FsdCooldown, "Ship.FsdCooldown");
                UpdateStatusProperty(_ship.LowFuel, raw.LowFuel, "Ship.LowFuel");
                UpdateStatusProperty(_ship.Overheating, raw.Overheating, "Ship.Overheating");
                UpdateStatusProperty(_ship.HasLatLong, raw.HasLatLong, "Ship.HasLatLong");
                UpdateStatusProperty(_ship.InDanger, raw.InDanger, "Ship.InDanger");
                UpdateStatusProperty(_ship.InInterdiction, raw.InInterdiction, "Ship.InIntediction");
                UpdateStatusProperty(_ship.InMothership, raw.InMothership, "Ship.InMothership");
                UpdateStatusProperty(_ship.InFighter, raw.InFighter, "Ship.InFighter");
                UpdateStatusProperty(_ship.InSrv, raw.InSrv, "Ship.InSrv");
                UpdateStatusProperty(_ship.AnalysisMode, raw.AnalysisMode, "Ship.AnalysisMode");
                UpdateStatusProperty(_ship.NightVision, raw.NightVision, "Ship.NightVision");
                UpdateStatusProperty(_ship.AltitudeFromAverageRadius, raw.AltitudeFromAverageRadius, "Ship.AltitudeFromAverageRadius");
                UpdateStatusProperty(_ship.FsdJump, raw.FsdJump, "Ship.FsdJump");
                UpdateStatusProperty(_ship.SrvHighBeam, raw.SrvHighBeam, "Ship.SrvHighBeam");

                UpdateStatusProperty(_ship.Pips, raw.Pips, "Ship.Pips");
                UpdateStatusProperty(_ship.FireGroup, raw.FireGroup, "Ship.FireGroup");
                UpdateStatusProperty(_ship.GuiFocus, raw.GuiFocus, "Ship.GuiFocus");
                UpdateStatusProperty(_ship.Fuel, raw.Fuel, "Ship.Fuel");
                UpdateStatusProperty(_ship.Cargo, raw.Cargo, "Ship.Cargo");
                UpdateStatusProperty(_ship.LegalState, raw.LegalState, "Ship.LegalState");
                UpdateStatusProperty(_ship.Latitude, raw.Latitude, "Ship.Latitude");
                UpdateStatusProperty(_ship.Altitude, raw.Altitude, "Ship.Altitude");
                UpdateStatusProperty(_ship.Longitude, raw.Longitude, "Ship.Longitude");
                UpdateStatusProperty(_ship.Heading, raw.Heading, "Ship.Heading");
                UpdateStatusProperty(_ship.Body, raw.Body, "Ship.Body");
                UpdateStatusProperty(_ship.BodyRadius, raw.BodyRadius, "Ship.BodyRadius");
                
                UpdateStatusProperty(_commander.Flags, raw.CommanderFlags, "Commander.Flags");
                UpdateStatusProperty(_commander.OnFoot, raw.OnFoot, "Commander.OnFoot");
                UpdateStatusProperty(_commander.InTaxi, raw.InTaxi, "Commander.InTaxi");
                UpdateStatusProperty(_commander.InMultiCrew, raw.InMultiCrew, "Commander.InMultiCrew");
                UpdateStatusProperty(_commander.OnFootInStation, raw.OnFootInStation, "Commander.OnFootInStation");
                UpdateStatusProperty(_commander.OnFootOnPlanet, raw.OnFootOnPlanet, "Commander.OnFootOnPlanet");
                UpdateStatusProperty(_commander.AimDownSight, raw.AimDownSight, "Commander.AimDownSight");
                UpdateStatusProperty(_commander.LowOxygen, raw.LowOxygen, "Commander.LowOxygen");
                UpdateStatusProperty(_commander.LowHealth, raw.LowHealth, "Commander.LowHealth");
                UpdateStatusProperty(_commander.Cold, raw.Cold, "Commander.Cold");
                UpdateStatusProperty(_commander.Hot, raw.Hot, "Commander.Hot");
                UpdateStatusProperty(_commander.VeryHot, raw.VeryHot, "Commander.VeryHot");
                UpdateStatusProperty(_commander.VeryHot, raw.VeryCold, "Commander.VeryCold");
                
                StatusUpdated?.Invoke(this, (content, raw));
            }
        }

        /// <inheritdoc />
        public async Task ProcessCargoFile(FileInfo cargoFile)
        {
            if (cargoFile == null || !cargoFile.Exists) return;

            var content = _fileReader.ReadAllText(cargoFile);
            if (!IsInCache(cargoFile, content))
            {
                AddToCache(cargoFile, content);
                var raw = await InvokeMethods<RawCargo>(content, _cargo);
                CargoUpdated?.Invoke(this, (content, raw));
            }
        }

        /// <inheritdoc />
        public async Task ProcessModulesFile(FileInfo modulesFile)
        {
            if (modulesFile == null || !modulesFile.Exists) return;

            var content = _fileReader.ReadAllText(modulesFile);
            if (!IsInCache(modulesFile, content))
            {
                AddToCache(modulesFile, content);
                var raw = await InvokeMethods<RawModules>(content, _modules);
                ModulesUpdated?.Invoke(this, (content, raw));
            }
        }

        /// <inheritdoc />
        public async Task ProcessMarketFile(FileInfo marketFile)
        {
            if (marketFile == null || !marketFile.Exists) return;

            var content = _fileReader.ReadAllText(marketFile);
            if (!IsInCache(marketFile, content))
            {
                AddToCache(marketFile, content);
                var raw = await InvokeMethods<RawMarket>(content, _market);
                MarketUpdated?.Invoke(this, (content, raw));
            }
        }

        /// <inheritdoc />
        public Task ProcessShipyardFile(FileInfo shipyardFile)
        {
            if (shipyardFile == null || !shipyardFile.Exists) return Task.CompletedTask;

            var content = _fileReader.ReadAllText(shipyardFile);
            if (!IsInCache(shipyardFile, content))
            {
                AddToCache(shipyardFile, content);
                //await InvokeShipyardMethods(content);
                ShipyardUpdated?.Invoke(this, content);
            }

            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public async Task ProcessOutfittingFile(FileInfo outfittingFile)
        {
            if (outfittingFile == null || !outfittingFile.Exists) return;

            var content = _fileReader.ReadAllText(outfittingFile);
            if (!IsInCache(outfittingFile, content))
            {
                AddToCache(outfittingFile, content);
                var raw = await InvokeMethods<RawOutfitting>(content, _outfitting);
                OutfittingUpdated?.Invoke(this, (content, raw));
            }
        }

        /// <inheritdoc />
        public async Task ProcessNavRouteFile(FileInfo navRouteFile)
        {
            if (navRouteFile == null || !navRouteFile.Exists) return;

            var content = _fileReader.ReadAllText(navRouteFile);
            if (!IsInCache(navRouteFile, content))
            {
                AddToCache(navRouteFile, content);
                var raw = await InvokeMethods<RawNavRoute>(content, _navRoute);
                NavRouteUpdated?.Invoke(this, (content, raw));
            }
        }

        private async Task<T> InvokeMethods<T>(string json, IStatus status)
        {
            var name = status.GetType().Name;

            try
            {
                var raw = JsonConvert.DeserializeObject<T>(json);

                foreach (var propertyName in status.GetType().GetProperties().Select(x => x.Name))
                    await InvokeUpdateMethod(raw, status, propertyName);

                status.TriggerOnChange();

                return raw;
            }
            catch (Exception ex)
            {
                _log.LogWarning(ex, "Could not update {name} status", name);
                return default;
            }
        }

        private Task InvokeUpdateMethod(object raw, object clean, string propertyName)
        {
            var name = propertyName;

            try
            {
                name = $"{clean.GetType().Name}.{name}";

                var rawValue = raw.GetType().GetProperty(propertyName).GetValue(raw);

                var value = rawValue.ToString();
                if (Type.GetTypeCode(rawValue.GetType()) == TypeCode.Object)
                    value = JsonConvert.SerializeObject(rawValue);

                var statusUpdateProperty = clean.GetType().GetProperty(propertyName).GetValue(clean);
                var updateMethod = statusUpdateProperty.GetType()
                    .GetMethod("Update", BindingFlags.NonPublic | BindingFlags.Instance);
                var needsUpdateMethod = statusUpdateProperty.GetType()
                    .GetMethod("NeedsUpdate", BindingFlags.NonPublic | BindingFlags.Instance);

                var needsUpdate = needsUpdateMethod.Invoke(statusUpdateProperty, new[] {rawValue});
                if ((bool) needsUpdate)
                {
                    _log.LogTrace("Invoking OnChange event for {name} ({value})", name, value);
                    updateMethod.Invoke(statusUpdateProperty, new[] {this, rawValue});
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("PropertyName", propertyName);
                _log.LogWarning(ex, "Could not invoke OnChange for {name}", name);
            }

            return Task.CompletedTask;
        }

        private void UpdateStatusProperty<T>(StatusProperty<T> property, object rawValue, string name)
        {
            try
            {
                if (property.NeedsUpdate(rawValue))
                {
                    var value = rawValue.ToString();
                    if (Type.GetTypeCode(rawValue.GetType()) == TypeCode.Object)
                        value = JsonConvert.SerializeObject(rawValue);
                    
                    _log.LogTrace("Invoking OnChange event for {name} ({value})", name, value);
                    property.Update(this, rawValue);
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("Name", name);
                _log.LogWarning(ex, "Could not invoke OnChange for {name}", name);
            }
        }

        private void AddToCache(FileSystemInfo file, string content)
        {
            if (!_cache.ContainsKey(file.Name))
                _cache.Add(file.Name, content);
            else
                _cache[file.Name] = content;
        }

        private bool IsInCache(FileSystemInfo file, string content)
        {
            return _cache.ContainsKey(file.Name) && _cache[file.Name] == content;
        }
    }
}