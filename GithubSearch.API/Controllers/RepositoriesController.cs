using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GithubSearch.DAL.Repository;
using GithubSearch.Shared.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GithubSearch.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RepositoriesController : ControllerBase
    {
        private readonly ILogger<RepositoriesController> _logger;
        private readonly ISearchEngine _searchEngine;
        private readonly IGithubRepositoriesRepository _githubRepositoriesRepository;

        // private readonly IActorRepository _actorRepository;
        public RepositoriesController(ILogger<RepositoriesController> logger, 
            ISearchEngine searchEngine, IGithubRepositoriesRepository githubRepositoriesRepository)
        {
            _logger = logger;
            _searchEngine = searchEngine;
            _githubRepositoriesRepository = githubRepositoriesRepository;
        }

        [HttpGet("search/{query}")]
        public async Task<IActionResult> GetSearch(string query)
        {
            IEnumerable<GithubSearch.Shared.DTO.Repository> repositories = new List<GithubSearch.Shared.DTO.Repository>(0);
            try
            {
                repositories = await _searchEngine.SearchAsync(query, CancellationToken.None).ConfigureAwait(false);
            }
            catch (System.Exception exp)
            {
                _logger.LogError("Error: Get Actors. {@exeption}", exp.Message);
            }

            return Ok(repositories);
        }
    }
}
