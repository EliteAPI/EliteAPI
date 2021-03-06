using System;

using EliteAPI.Event.Models;
using EliteAPI.Event.Models.Abstractions;

using Newtonsoft.Json;

using ProtoBuf;

namespace EliteAPI.Event.Models
{

    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class WingAddEvent : EventBase<WingAddEvent>
    {
        internal WingAddEvent() { }

        [JsonProperty("Name")]
        public string Name { get; private set; }
    }

}

namespace EliteAPI.Event.Handler
{
    public partial class EventHandler
    {
        public event EventHandler<WingAddEvent> WingAddEvent;

        internal void InvokeWingAddEvent(WingAddEvent arg)
        {
            WingAddEvent?.Invoke(this, arg);
        }
    }
}