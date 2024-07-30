using System;
using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PostRunner
{
    public partial class TaskConfig
    {
        [JsonProperty("post_objects")]
        public PostObject[] PostObjects { get; set; }

        [JsonProperty("transport_bones")]
        public object[] TransportBones { get; set; }

        [JsonProperty("transport_routes")]
        public object[] TransportRoutes { get; set; }

        [JsonProperty("version")]
        public Version Version { get; set; }
    }

    public partial class PostObject
    {
        [JsonProperty("gates")]
        public string[] Gates { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("pid")]
        public string Pid { get; set; }

        [JsonProperty("index")]
        public string Index { get; set; }

        [JsonProperty("route")]
        public System.Collections.Generic.Dictionary<string, string> Route { get; set; }

        [JsonProperty("typepostoffice_id")]
        public long TypepostofficeId { get; set; }
    }


    public partial class Version
    {
        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("timestamp")]
        public DateTimeOffset Timestamp { get; set; }
    }

    public partial class TaskConfig
    {
        public static TaskConfig FromJson(string json) => JsonConvert.DeserializeObject<TaskConfig>(json, Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this TaskConfig self) => JsonConvert.SerializeObject(self, Converter.Settings);
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
