﻿using System;

using EliteAPI.Event.Models;
using EliteAPI.Event.Models.Abstractions;

using Newtonsoft.Json;

using ProtoBuf;

namespace EliteAPI.Event.Models
{

    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class SquadronDisbandedEvent : EventBase<SquadronDisbandedEvent>
    {
        internal SquadronDisbandedEvent() { }

        [JsonProperty("SquadronName")]
        public string Name { get; internal set; }
    }

}

namespace EliteAPI.Event.Handler
{
    public partial class EventHandler
    {
        public event EventHandler<SquadronDisbandedEvent> SquadronDisbandedEvent;

        internal void InvokeSquadronDisbandedEvent(SquadronDisbandedEvent arg)
        {
            SquadronDisbandedEvent?.Invoke(this, arg);
        }
    }
}