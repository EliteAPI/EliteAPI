using System;

using EliteAPI.Event.Models;
using EliteAPI.Event.Models.Abstractions;

using Newtonsoft.Json;

using ProtoBuf;

namespace EliteAPI.Event.Models
{

    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class WingLeaveEvent : EventBase<WingLeaveEvent>
    {
        internal WingLeaveEvent() { }

        [JsonProperty("event")]
        public string Event { get; private set; }
    }

}

namespace EliteAPI.Event.Handler
{
    public partial class EventHandler
    {
        public event EventHandler<WingLeaveEvent> WingLeaveEvent;

        internal void InvokeWingLeaveEvent(WingLeaveEvent arg)
        {
            WingLeaveEvent?.Invoke(this, arg);
        }
    }
}