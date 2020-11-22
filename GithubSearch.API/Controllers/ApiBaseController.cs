using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GithubSearch.API.Controllers
{
    public class ApiBaseController : ControllerBase
    {
        protected string UserId
        {
            get
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                return userId;
            }
        }
    }
}
