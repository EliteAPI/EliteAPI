using System;

using EliteAPI.Event.Models;
using EliteAPI.Event.Models.Abstractions;

using Newtonsoft.Json;

using ProtoBuf;

namespace EliteAPI.Event.Models
{

    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class PayLegacyFinesEvent : EventBase<PayLegacyFinesEvent>
    {
        internal PayLegacyFinesEvent() { }

        [JsonProperty("Amount")]
        public long Amount { get; private set; }

        [JsonProperty("BrokerPercentage")]
        public double BrokerPercentage { get; private set; }
    }

}

namespace EliteAPI.Event.Handler
{
    public partial class EventHandler
    {
        public event EventHandler<PayLegacyFinesEvent> PayLegacyFinesEvent;

        internal void InvokePayLegacyFinesEvent(PayLegacyFinesEvent arg)
        {
            PayLegacyFinesEvent?.Invoke(this, arg);
        }
    }
}