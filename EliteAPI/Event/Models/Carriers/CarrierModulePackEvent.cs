﻿using System;

using EliteAPI.Event.Models;
using EliteAPI.Event.Models.Abstractions;

using Newtonsoft.Json;

using ProtoBuf;

namespace EliteAPI.Event.Models
{

    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class CarrierModulePackEvent : EventBase<CarrierModulePackEvent>
    {
        internal CarrierModulePackEvent() { }

        [JsonProperty("CarrierID")]
        public string CarrierId { get; private set; }

        [JsonProperty("Operation")]
        public string Operation { get; private set; }

        [JsonProperty("PackTheme")]
        public string Theme { get; private set; }

        [JsonProperty("PackTier")]
        public int Tier { get; private set; }

        [JsonProperty("Cost")]
        public long Cost { get; private set; }

        [JsonProperty("Refund")]
        public long Refund { get; private set; }
    }

}

namespace EliteAPI.Event.Handler
{
    public partial class EventHandler
    {
        public event EventHandler<CarrierModulePackEvent> CarrierModulePackEvent;

        internal void InvokeCarrierModulePackEvent(CarrierModulePackEvent arg)
        {
            CarrierModulePackEvent?.Invoke(this, arg);
        }
    }
}