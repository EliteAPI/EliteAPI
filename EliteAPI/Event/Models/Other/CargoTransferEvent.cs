using System;
using System.Collections.Generic;

using EliteAPI.Event.Models;
using EliteAPI.Event.Models.Abstractions;

using Newtonsoft.Json;

using ProtoBuf;

namespace EliteAPI.Event.Models
{

    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class CargoTransferEvent : EventBase<CargoTransferEvent>
    {
        internal CargoTransferEvent() { }

        [JsonProperty("Transfers")]
        public IReadOnlyList<Transfer> Transfers { get; private set; }
    }

    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class Transfer
    {
        internal Transfer() { }

        [JsonProperty("Type")]
        public string Type { get; private set; }

        [JsonProperty("Count")]
        public long Count { get; private set; }

        [JsonProperty("Direction")]
        public string Direction { get; private set; }
    }

}

namespace EliteAPI.Event.Handler
{
    public partial class EventHandler
    {
        public event EventHandler<CargoTransferEvent> CargoTransferEvent;

        internal void InvokeCargoTransferEvent(CargoTransferEvent arg)
        {
            CargoTransferEvent?.Invoke(this, arg);
        }
    }
}