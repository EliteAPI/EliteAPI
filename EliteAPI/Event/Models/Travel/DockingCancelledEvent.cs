using System;

using EliteAPI.Event.Models;
using EliteAPI.Event.Models.Abstractions;

using Newtonsoft.Json;

using ProtoBuf;

namespace EliteAPI.Event.Models
{

    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class DockingCancelledEvent : EventBase<DockingCancelledEvent>
    {
        internal DockingCancelledEvent() { }

        [JsonProperty("StationName")]
        public string StationName { get; private set; }
    }

}

namespace EliteAPI.Event.Handler
{
    public partial class EventHandler
    {
        public event EventHandler<DockingCancelledEvent> DockingCancelledEvent;

        internal void InvokeDockingCancelledEvent(DockingCancelledEvent arg)
        {
            DockingCancelledEvent?.Invoke(this, arg);
        }
    }
}