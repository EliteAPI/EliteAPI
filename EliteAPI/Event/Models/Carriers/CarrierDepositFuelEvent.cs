using System;

using EliteAPI.Event.Models;
using EliteAPI.Event.Models.Abstractions;

using Newtonsoft.Json;

using ProtoBuf;

namespace EliteAPI.Event.Models
{

    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class CarrierDepositFuelEvent : EventBase<CarrierDepositFuelEvent>
    {
        internal CarrierDepositFuelEvent() { }

        [JsonProperty("CarrierID")]
        public string CarrierId { get; private set; }

        [JsonProperty("Amount")]
        public int Amount { get; private set; }

        [JsonProperty("Total")]
        public int Total { get; private set; }
    }


}

namespace EliteAPI.Event.Handler
{

    public partial class EventHandler
    {
        public event EventHandler<CarrierDepositFuelEvent> CarrierDepositFuelEvent;
        
        internal void InvokeCarrierDepositFuelEvent(CarrierDepositFuelEvent arg)
        {
            CarrierDepositFuelEvent?.Invoke(this, arg);
        }
    }
}