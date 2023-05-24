namespace AppService.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Services;
    using System;
    using System.Threading.Tasks;

    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AppSettingsController : ControllerBase
    {
        private readonly IAppSettingsService _appSettingsService;

        public AppSettingsController(IAppSettingsService appSettingsService)
        {
            _appSettingsService = appSettingsService ?? throw new ArgumentNullException(nameof(appSettingsService));
        }

        [HttpGet]
        public virtual async Task<AppSettings> GetAsync()
        {
            return await _appSettingsService.GetAsync().ConfigureAwait(false);
        }
    }
}
