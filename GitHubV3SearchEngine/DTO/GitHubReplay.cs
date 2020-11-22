using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace GitHubSearchEngine.DTO
{
    class GitHubReplay
    {
        [JsonProperty("total_count")]
        public int TotalCount { get; set; }
        [JsonProperty("incomplete_results")]
        public bool IncompleteResults { get; set; }

        [JsonProperty("items")]
        public IEnumerable<Repository> repositories { get; set; }
    }
}
