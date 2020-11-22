using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.Json.Serialization;

namespace GitHubSearchEngine.DTO
{
    public class Repository
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("html_url")]
        public Uri GitHubHomeUrl { get; set; }

        [JsonProperty("homepage")]
        public Uri Homepage { get; set; }

        [JsonProperty("watchers")]
        public int Watchers { get; set; }

        [JsonProperty("pushed_at")]
        public string JsonDate { get; set; }

        public DateTime LastPush =>
            DateTime.ParseExact(JsonDate, "yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture);
    }
}
