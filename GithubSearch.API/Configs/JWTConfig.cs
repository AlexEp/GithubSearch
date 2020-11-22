using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GithubSearch.API.Configs
{
    public class JWTConfig
    {
        public string SecretKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }
}
