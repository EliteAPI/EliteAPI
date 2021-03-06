using System;

using EliteAPI.Event.Models;
using EliteAPI.Event.Models.Abstractions;

using Newtonsoft.Json;

using ProtoBuf;

namespace EliteAPI.Event.Models
{

    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class FetchRemoteModuleEvent : EventBase<FetchRemoteModuleEvent>
    {
        internal FetchRemoteModuleEvent() { }

        [JsonProperty("StorageSlot")]
        public string StorageSlot { get; private set; }

        [JsonProperty("StoredItem")]
        public string StoredItem { get; private set; }

        [JsonProperty("StoredItem_Localised")]
        public string StoredItemLocalised { get; private set; }

        [JsonProperty("ServerId")]
        public string ServerId { get; private set; }

        [JsonProperty("TransferCost")]
        public long TransferCost { get; private set; }

        [JsonProperty("TransferTime")]
        public long TransferTime { get; private set; }

        [JsonProperty("Ship")]
        public string Ship { get; private set; }

        [JsonProperty("ShipID")]
        public string ShipId { get; private set; }
    }

}

namespace EliteAPI.Event.Handler
{
    public partial class EventHandler
    {
        public event EventHandler<FetchRemoteModuleEvent> FetchRemoteModuleEvent;

        internal void InvokeFetchRemoteModuleEvent(FetchRemoteModuleEvent arg)
        {
            FetchRemoteModuleEvent?.Invoke(this, arg);
        }
    }
}