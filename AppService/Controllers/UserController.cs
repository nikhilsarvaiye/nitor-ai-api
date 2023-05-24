namespace AppService.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.IdentityModel.Tokens;
    using Models;
    using Newtonsoft.Json.Linq;
    using Services;
    using System;
    using System.Text.Json;
    using System.Threading.Tasks;

    [ApiController]
    [Route("[controller]")]
    public class UserController : BaseController<User>
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
            : base(userService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        [HttpPut("update-theme/{id}/{theme}")]
        public async Task UpdateThemeAsync(string id, string theme)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (string.IsNullOrEmpty(theme))
            {
                throw new ArgumentNullException(nameof(theme));
            }

            await _userService.UpdateThemeAsync(id, theme);
        }

        [HttpPost("login")]
        public async Task<LoggedInUser> LogInAsync((string UserId, string Password) model)
        {
            if (string.IsNullOrEmpty(model.UserId))
            {
                throw new ArgumentNullException(nameof(model.UserId));
            }

            if (string.IsNullOrEmpty(model.Password))
            {
                throw new ArgumentNullException(nameof(model.Password));
            }

            return await _userService.LogInAsync(model.UserId, model.Password);
        }
    }
}
