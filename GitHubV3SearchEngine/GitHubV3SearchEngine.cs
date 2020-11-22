using AutoMapper;
using GithubSearch.Shared.Interfaces;
using GitHubSearchEngine.AutoMapper;
using GitHubSearchEngine.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace GitHubSearchEngine
{
    public class GitHubV3SearchEngine : ISearchEngine
    {
        protected static readonly IMapper _mapper;

        private  const string SITE_URL = "https://api.github.com/search/repositories";

        static GitHubV3SearchEngine()
        {
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutoMapperProfiles());
            });

            _mapper = mapperConfig.CreateMapper();
        }


        public async Task<IEnumerable<GithubSearch.Shared.DTO.Repository>> SearchAsync(string searchValue, CancellationToken cancellationToken)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(
                         new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Add("user-agent", "OnlyTest");

            HttpResponseMessage response = await httpClient.GetAsync($"https://api.github.com/search/repositories?q={searchValue}", cancellationToken).ConfigureAwait(false);
            cancellationToken.ThrowIfCancellationRequested();

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var repositories = JsonConvert.DeserializeObject<GitHubReplay>(json);

            var entity = _mapper.Map<IEnumerable<GithubSearch.Shared.DTO.Repository>>(repositories.repositories);
            return entity;
        }
    }
}
