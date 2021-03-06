using System;

using EliteAPI.Event.Models;
using EliteAPI.Event.Models.Abstractions;

using Newtonsoft.Json;

using ProtoBuf;

namespace EliteAPI.Event.Models
{

    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class PowerplayLeaveEvent : EventBase<PowerplayLeaveEvent>
    {
        internal PowerplayLeaveEvent() { }

        [JsonProperty("Power")]
        public string Power { get; private set; }
    }

}

namespace EliteAPI.Event.Handler
{
    public partial class EventHandler
    {
        public event EventHandler<PowerplayLeaveEvent> PowerplayLeaveEvent;

        internal void InvokePowerplayLeaveEvent(PowerplayLeaveEvent arg)
        {
            PowerplayLeaveEvent?.Invoke(this, arg);
        }
    }
}