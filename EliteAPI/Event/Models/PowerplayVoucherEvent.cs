using System.Collections.Generic;
using Newtonsoft.Json;

namespace EliteAPI.Event.Models
{
    public class PowerplayVoucherEvent : EventBase
    {
        internal PowerplayVoucherEvent() { }
        [JsonProperty("Power")]
        public string Power { get; internal set; }

        [JsonProperty("Systems")]
        public List<string> Systems { get; internal set; }

        
    }
}