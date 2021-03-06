﻿using System;
using System.Reflection;
using System.Threading.Tasks;

using EliteAPI.Event.Models.Abstractions;
using EliteAPI.Event.Processor.Abstractions;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using EventHandler = EliteAPI.Event.Handler.EventHandler;

namespace EliteAPI.Event.Processor
{
    internal class AllEventProcessor : IEventProcessor
    {
        private readonly EventHandler _eventHandler;
        private readonly ILogger<AllEventProcessor> _log;
        private MethodBase _invokeMethod;

        public AllEventProcessor(ILogger<AllEventProcessor> log, IServiceProvider services)
        {
            _log = log;
            _eventHandler = services.GetRequiredService<EventHandler>();
        }

        public Task RegisterHandlers()
        {
            return Task.CompletedTask;
        }

        public Task InvokeHandler(IEvent gameEvent, bool isWhileCatchingUp)
        {
            try
            {
                _log.LogTrace("Invoking AllEvent for {event}", gameEvent.Event);
                _eventHandler.InvokeAllEvent(gameEvent);
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Could not invoke method for {event}", gameEvent.Event);
                return Task.CompletedTask;
            }
        }
    }
}