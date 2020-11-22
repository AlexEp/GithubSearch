using GithubSearch.API.Identity;
using GithubSearch.Shared.DTO;
using GithubSearch.Shared.Services;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GithubSearch.API.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppIdentityUser> _userManager;
        private readonly SignInManager<AppIdentityUser> _signInManager;

        public UserService(
            UserManager<AppIdentityUser> userManager,
            SignInManager<AppIdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<AppUser> GetUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return null;

            return new AppUser()
            {
                Id = user.Id,
                UserName = user.UserName,
                FirstName = user.FirstName,
                Email = user.Email,
            };
        }

        public async Task<AppUser> CreateAsync(AppUser user, string password)
        {
            var foundUser = await _userManager.FindByNameAsync(user.UserName);

            if (foundUser != null)
                throw new InvalidOperationException("Username already exists");

            AppIdentityUser userToCreate = new AppIdentityUser(user.UserName)
            {
                FirstName = user.FirstName,
                Email = user.Email,
            };
            IdentityResult result = await _userManager.CreateAsync(userToCreate, password);

            if (!result.Succeeded)
            {
                throw new AggregateException(result.Errors.Select(e => new InvalidOperationException(e.Description)));
            }

            user.Id = userToCreate.Id;
            return user;
        }



        public async Task<AppUser> GetUserAsync(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user != null && await _userManager.CheckPasswordAsync(user, password))
            {
                return new AppUser()
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    Email = user.Email,
                };
            }

            return null;

        }
    }
}
