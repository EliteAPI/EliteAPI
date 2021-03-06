using System;

using EliteAPI.Event.Models;
using EliteAPI.Event.Models.Abstractions;

using Newtonsoft.Json;

using ProtoBuf;

namespace EliteAPI.Event.Models
{

    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class RepairAllEvent : EventBase<RepairAllEvent>
    {
        internal RepairAllEvent() { }

        [JsonProperty("Cost")]
        public long Cost { get; private set; }
    }

}

namespace EliteAPI.Event.Handler
{
    public partial class EventHandler
    {
        public event EventHandler<RepairAllEvent> RepairAllEvent;

        internal void InvokeRepairAllEvent(RepairAllEvent arg)
        {
            RepairAllEvent?.Invoke(this, arg);
        }
    }
}