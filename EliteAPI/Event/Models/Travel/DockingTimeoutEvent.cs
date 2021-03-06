using System;

using EliteAPI.Event.Models;
using EliteAPI.Event.Models.Abstractions;

using Newtonsoft.Json;

using ProtoBuf;

namespace EliteAPI.Event.Models
{

    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class DockingTimeoutEvent : EventBase<DockingTimeoutEvent>
    {
        internal DockingTimeoutEvent() { }

        [JsonProperty("StationName")]
        public string StationName { get; private set; }
    }

}

namespace EliteAPI.Event.Handler
{
    public partial class EventHandler
    {
        public event EventHandler<DockingTimeoutEvent> DockingTimeoutEvent;

        internal void InvokeDockingTimeoutEvent(DockingTimeoutEvent arg)
        {
            DockingTimeoutEvent?.Invoke(this, arg);
        }
    }
}