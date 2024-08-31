using System;
using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PostModel
{
    [Serializable]
    public partial class TaskConfig
    {
        [JsonProperty("post_objects")]
        public PostObject[] PostObjects { get; set; }

        [JsonProperty("transport_bones")]
        public TransportBone[] TransportBones { get; set; }

        [JsonProperty("transport_routes")]
        public TransportRoute[] TransportRoutes { get; set; }

        [JsonProperty("version")]
        public Version Version { get; set; }

        [JsonProperty("in_data")]
        public InData[] InDates { get; set; }
    }

    [Serializable]
    public partial class TransportRoute
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("shedule")]
        public Dictionary<string, long> Shedule { get; set; }

        [JsonProperty("start_time")]
        public double StartTime { get; set; }
    }

    [Serializable]
    public class TransportBone
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("start_id")]
        public string Start_id { get; set; }

        [JsonProperty("end_id")]
        public string End_id { get; set; }

        [JsonProperty("time")]
        public double Time { get; set; }

        [JsonProperty("distance")]
        public double Distance { get; set; }

        [JsonProperty("price")]
        public double Price { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }

    [Serializable]
    public partial class PostObject
    {
        [JsonProperty("fix_price")]
        public long FixPrice { get; set; }

        [JsonProperty("gates")]
        public string[] Gates { get; set; }

        [JsonProperty("geo_lat")]
        public double GeoLat { get; set; }

        [JsonProperty("geo_lon")]
        public double GeoLon { get; set; }

        [JsonProperty("hour_price")]
        public double HourPrice { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("index")]
        public string Index { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("pid")]
        public long Pid { get; set; }

        [JsonProperty("route")]
        public Dictionary<string, Dictionary<string, string?>> Route { get; set; }

        [JsonProperty("su_type")]
        public string SuType { get; set; }

        [JsonProperty("typepostoffice_id")]
        public long TypepostofficeId { get; set; }
    }

    [Serializable]
    public partial class InData
    {
        [JsonProperty("end_index")]
        public string EndIndex { get; set; }

        [JsonProperty("group_prediction")]
        public string GroupPrediction { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("ks")]
        public long Ks { get; set; }

        [JsonProperty("start_index")]
        public string StartIndex { get; set; }

        [JsonProperty("type_message_id")]
        public string TypeMessageId { get; set; }

        [JsonProperty("weight")]
        public double Weight { get; set; }

        [JsonProperty("weight_result")]
        public double WeightResult { get; set; }
    }

    [Serializable]
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
