using System;

using EliteAPI.Event.Models;
using EliteAPI.Event.Models.Abstractions;

using ProtoBuf;

namespace EliteAPI.Event.Models
{

    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class HeatDamageEvent : EventBase<HeatDamageEvent>
    {
        internal HeatDamageEvent() { }
    }

}

namespace EliteAPI.Event.Handler
{
    public partial class EventHandler
    {
        public event EventHandler<HeatDamageEvent> HeatDamageEvent;

        internal void InvokeHeatDamageEvent(HeatDamageEvent arg)
        {
            HeatDamageEvent?.Invoke(this, arg);
        }
    }
}