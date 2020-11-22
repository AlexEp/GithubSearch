using GithubSearch.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GithubSearch.Shared.Services
{
    public interface IUserService
    {
        Task<AppUser> GetUserAsync(string id);
        Task<AppUser> GetUserAsync(string username, string password);
        Task<AppUser> CreateAsync(AppUser user, string password);
        //User RegisterUser(string username, string password);
        //DeleteUser(string username, string password);
    }
}
