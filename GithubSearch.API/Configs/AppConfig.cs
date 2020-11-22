using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GithubSearch.API.Configs
{
    public class AppConfig
    {
        public CORSConfig CORS { get; set; }
        public JWTConfig JWT { get; set; }
        public DBConfig DB { get; set; }
    }
}
