﻿using System;
using System.Collections.Generic;
using EliteAPI.Event.Models;
using EliteAPI.Event.Models.Abstractions;

using Newtonsoft.Json;

using ProtoBuf;

namespace EliteAPI.Event.Models
{

    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class TransferMicroResourcesEvent : EventBase<TransferMicroResourcesEvent>
    {
        internal TransferMicroResourcesEvent() { }

        [JsonProperty("Transfers")]
        public IReadOnlyList<MicroResourceInfo> Transfers { get; private set; }

        [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
        public class MicroResourceInfo
        {
            internal MicroResourceInfo() { }

            [JsonProperty("Name")]
            public string Name { get; private set; }

            [JsonProperty("Name_Localised")]
            public string NameLocalised { get; private set; }

            [JsonProperty("Category")]
            public string Category { get; private set; }

            [JsonProperty("Count")]
            public long Count { get; private set; }

            [JsonProperty("Direction")]
            public string Direction { get; private set; }
        }
    }
}

namespace EliteAPI.Event.Handler
{
    public partial class EventHandler
    {
        public event EventHandler<TransferMicroResourcesEvent> TransferMicroResourcesEvent;

        internal void InvokeTransferMicroResourcesEvent(TransferMicroResourcesEvent arg)
        {
            TransferMicroResourcesEvent?.Invoke(this, arg);
        }
    }
}