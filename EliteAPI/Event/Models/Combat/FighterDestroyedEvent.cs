using System;

using EliteAPI.Event.Models;
using EliteAPI.Event.Models.Abstractions;

using Newtonsoft.Json;

using ProtoBuf;

namespace EliteAPI.Event.Models
{

    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class FighterDestroyedEvent : EventBase<FighterDestroyedEvent>
    {
        internal FighterDestroyedEvent() { }

        [JsonProperty("ID")]
        public string Id { get; private set; }
    }

}

namespace EliteAPI.Event.Handler
{
    public partial class EventHandler
    {
        public event EventHandler<FighterDestroyedEvent> FighterDestroyedEvent;

        internal void InvokeFighterDestroyedEvent(FighterDestroyedEvent arg)
        {
            FighterDestroyedEvent?.Invoke(this, arg);
        }
    }
}