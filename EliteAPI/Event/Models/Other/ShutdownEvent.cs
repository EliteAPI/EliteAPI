using System;

using EliteAPI.Event.Models;
using EliteAPI.Event.Models.Abstractions;

using Newtonsoft.Json;

using ProtoBuf;

namespace EliteAPI.Event.Models
{

    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class ShutdownEvent : EventBase<ShutdownEvent>
    {
        internal ShutdownEvent() { }

        [JsonProperty("event")]
        public string Event { get; private set; }
    }

}

namespace EliteAPI.Event.Handler
{
    public partial class EventHandler
    {
        public event EventHandler<ShutdownEvent> ShutdownEvent;

        internal void InvokeShutdownEvent(ShutdownEvent arg)
        {
            ShutdownEvent?.Invoke(this, arg);
        }
    }
}