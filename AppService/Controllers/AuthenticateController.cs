namespace AppService.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Services;
    using System;
    using System.Text.Json.Nodes;
    using System.Threading.Tasks;

    [ApiController]
    [Route("[controller]")]
    public class AuthenticateController
    {
        private readonly IUserService _userService;

        public AuthenticateController(IUserService userService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        [HttpPost("register")]
        public virtual async Task<LoggedInUser> RegisterAsync(User user)
        {
            return await _userService.RegisterAsync(user).ConfigureAwait(false);
        }

        [HttpPost("login")]
        public async Task<LoggedInUser> LogInAsync(JsonNode jsonNode)
        {
            if (string.IsNullOrEmpty(jsonNode["userId"]?.ToString()))
            {
                throw new ArgumentNullException("userId");
            }

            if (string.IsNullOrEmpty(jsonNode["password"]?.ToString()))
            {
                throw new ArgumentNullException("password");
            }

            return await _userService.LogInAsync(jsonNode["userId"]?.ToString(), jsonNode["password"]?.ToString());
        }
    }
}
