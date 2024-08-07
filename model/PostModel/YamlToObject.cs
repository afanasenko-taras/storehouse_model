using System;
using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PostModel
{
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

    public partial class TransportRoute
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("shedule")]
        public Dictionary<string, long> Shedule { get; set; }

        [JsonProperty("start_time")]
        public double StartTime { get; set; }
    }

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

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("route")]
        public System.Collections.Generic.Dictionary<string, string> Route { get; set; }

        [JsonProperty("typepostoffice_id")]
        public long TypepostofficeId { get; set; }

        [JsonProperty("su_type")]
        public string SuType { get; set; }

        [JsonProperty("geo_lat")]
        public double GeoLat { get; set; }


        [JsonProperty("geo_lon")]
        public double GeoLon { get; set; }

    }

    public partial class InData
    {
        [JsonProperty("start_index")]
        public string StartIndex { get; set; }

        [JsonProperty("end_index")]
        public string EndIndex { get; set; }

        [JsonProperty("group_prediction")]
        public string GroupPrediction { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("ks")]
        public double Ks { get; set; }

        [JsonProperty("weight")]
        public double Weight { get; set; }
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
