using AutoMapper;
using GithubSearch.API.Configs;
using GithubSearch.API.Models;
using GithubSearch.Shared.DTO;
using GithubSearch.Shared.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GithubSearch.API.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ApiBaseController
    {
        private readonly IUserService _userService;
        private readonly ILogger<AuthController> _logger;
        private readonly AppConfig _appSettings;
        private readonly IMapper _mapper;

        public AuthController(IOptions<AppConfig> appSettings,
                 IMapper mapper,
                 IUserService userService,
                 ILogger<AuthController> logger)
        {
            _mapper = mapper;
            _appSettings = appSettings.Value;
            _userService = userService;
            _logger = logger;
        }


        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(UserRegisterModal userRegisterModal)
        {
            userRegisterModal.Username = userRegisterModal.Username.ToLower();

            var userToCreate = _mapper.Map<AppUser>(userRegisterModal);

            try
            {
                var user = await _userService.CreateAsync(userToCreate, userRegisterModal.Password);
                _logger.LogInformation("New user was created. UserName: {userName}: ", user.UserName);
                return Ok(user);
            }
            catch (AggregateException exp)
            {
                var errorMsg = string.Join(",", exp.Flatten());
                _logger.LogError(exp, errorMsg);
                return BadRequest(errorMsg);
            }
            catch (Exception exp)
            {
                _logger.LogError(exp, exp.Message);
                return BadRequest(exp.Message);
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginModel model)
        {
            var user = await _userService.GetUserAsync(model.UserName, model.Password);

            try
            {
                if (user != null)
                {
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new Claim[]
                        {
                        new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                        new Claim("UserName",user.UserName)
                        }),
                        Expires = DateTime.UtcNow.AddDays(1),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.JWT.SecretKey)), SecurityAlgorithms.HmacSha256Signature)
                    };
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                    var token = tokenHandler.WriteToken(securityToken);
                    return Ok(new { token });
                }
                else
                    return BadRequest(new { message = "Username or password is incorrect." });
            }
            catch (Exception exp)
            {
                _logger.LogError(exp, exp.Message);
                return BadRequest(exp.Message);
            }

        }
    }
}
