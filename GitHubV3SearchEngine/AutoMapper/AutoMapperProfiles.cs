using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace GitHubSearchEngine.AutoMapper
{
    class AutoMapperProfiles : Profile
    {

        public AutoMapperProfiles()
        {
            CreateMap<GithubSearch.Shared.DTO.Repository, GitHubSearchEngine.DTO.Repository>();
            CreateMap<GitHubSearchEngine.DTO.Repository, GithubSearch.Shared.DTO.Repository>();
        }  
    }
}
