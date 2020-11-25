
namespace EliteAPI.Event.Models
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using Abstractions;


    public partial class RankEvent : EventBase
    {
        internal RankEvent() { }

        [JsonProperty("Combat")]
        public long Combat { get; private set; }

        [JsonProperty("Trade")]
        public long Trade { get; private set; }

        [JsonProperty("Explore")]
        public long Explore { get; private set; }

        [JsonProperty("Empire")]
        public long Empire { get; private set; }

        [JsonProperty("Federation")]
        public long Federation { get; private set; }

        [JsonProperty("CQC")]
        public long Cqc { get; private set; }
    }

    public partial class RankEvent
    {
        public static RankEvent FromJson(string json) => JsonConvert.DeserializeObject<RankEvent>(json);
    }

    
}

namespace EliteAPI.Event.Handler
{
    using System;
    using Models;

    public partial class EventHandler
    {
        public event EventHandler<RankEvent> RankEvent;
        internal void InvokeRankEvent(RankEvent arg) => RankEvent?.Invoke(this, arg);
    }
}