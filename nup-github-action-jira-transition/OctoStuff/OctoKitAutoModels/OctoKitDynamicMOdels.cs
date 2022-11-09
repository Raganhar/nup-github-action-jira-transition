﻿using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

// <auto-generated />
//
// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using DotNet.GitHubAction.OctoStuff.OctoKitAutoModels;
//
//    var welcome = Welcome.FromJson(jsonString);

namespace DotNet.GitHubAction.OctoStuff.OctoKitAutoModels
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class graphQlResult
    {
        [JsonProperty("data")]
        public Data Data { get; set; }
    }

    public partial class Data
    {
        [JsonProperty("repository")]
        public Repository Repository { get; set; }
    }

    public partial class Repository
    {
        [JsonProperty("ref")]
        public Ref Ref { get; set; }
    }

    public partial class Ref
    {
        [JsonProperty("target")]
        public Target Target { get; set; }
    }

    public partial class Target
    {
        [JsonProperty("history")]
        public History History { get; set; }
    }

    public partial class History
    {
        [JsonProperty("edges")]
        public HistoryEdge[] Edges { get; set; }
    }

    public partial class HistoryEdge
    {
        [JsonProperty("node")]
        public PurpleNode Node { get; set; }
    }

    public partial class PurpleNode
    {
        [JsonProperty("committedDate")]
        public DateTimeOffset CommittedDate { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("oid")]
        public string Oid { get; set; }

        [JsonProperty("url")]
        public Uri Url { get; set; }

        [JsonProperty("parents")]
        public Parents Parents { get; set; }
    }

    public partial class Parents
    {
        [JsonProperty("edges")]
        public ParentsEdge[] Edges { get; set; }
    }

    public partial class ParentsEdge
    {
        [JsonProperty("node")]
        public FluffyNode Node { get; set; }
    }

    public partial class FluffyNode
    {
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("oid")]
        public string Oid { get; set; }
    }

    public partial class graphQlResult
    {
        public static graphQlResult FromJson(string json) => JsonConvert.DeserializeObject<graphQlResult>(json, DotNet.GitHubAction.OctoStuff.OctoKitAutoModels.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this graphQlResult self) => JsonConvert.SerializeObject(self, DotNet.GitHubAction.OctoStuff.OctoKitAutoModels.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}
