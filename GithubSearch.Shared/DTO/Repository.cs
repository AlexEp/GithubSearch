using System;
using System.Collections.Generic;
using System.Text;

namespace GithubSearch.Shared.DTO
{
    public class Repository
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Uri GitHubHomeUrl { get; set; }
        public Uri Homepage { get; set; }
        public int Watchers { get; set; }
        public DateTime LastPush { get; set; }
    }
}
