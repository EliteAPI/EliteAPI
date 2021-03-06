using System;

using EliteAPI.Event.Models;
using EliteAPI.Event.Models.Abstractions;

using Newtonsoft.Json;

using ProtoBuf;

namespace EliteAPI.Event.Models
{

    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class ScannedEvent : EventBase<ScannedEvent>
    {
        internal ScannedEvent() { }

        [JsonProperty("ScanType")]
        public string ScanType { get; private set; }
    }

}

namespace EliteAPI.Event.Handler
{
    public partial class EventHandler
    {
        public event EventHandler<ScannedEvent> ScannedEvent;

        internal void InvokeScannedEvent(ScannedEvent arg)
        {
            ScannedEvent?.Invoke(this, arg);
        }
    }
}