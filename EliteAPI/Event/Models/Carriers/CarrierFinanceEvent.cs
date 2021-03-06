﻿using System;

using EliteAPI.Event.Models;
using EliteAPI.Event.Models.Abstractions;

using Newtonsoft.Json;

using ProtoBuf;

namespace EliteAPI.Event.Models
{

    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class CarrierFinanceEvent : EventBase<CarrierFinanceEvent>
    {
        internal CarrierFinanceEvent() { }

        [JsonProperty("CarrierID")]
        public string CarrierId { get; private set; }

        [JsonProperty("TaxRate")]
        public int TaxRate { get; private set; }

        [JsonProperty("CarrierBalance")]
        public long Balance { get; private set; }

        [JsonProperty("ReserveBalance")]
        public long ReserveBalance { get; private set; }

        [JsonProperty("AvailableBalance")]
        public long AvailableBalance { get; private set; }

        [JsonProperty("ReservePercent")]
        public int ReservePercent { get; private set; }
    }

}

namespace EliteAPI.Event.Handler
{
    public partial class EventHandler
    {
        public event EventHandler<CarrierFinanceEvent> CarrierFinanceEvent;

        internal void InvokeCarrierFinanceEvent(CarrierFinanceEvent arg)
        {
            CarrierFinanceEvent?.Invoke(this, arg);
        }
    }
}