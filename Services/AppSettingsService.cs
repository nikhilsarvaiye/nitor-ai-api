namespace Services
{
    using Models;
    using System.Linq;
    using System.Threading.Tasks;

    public class AppSettingsService : IAppSettingsService
    {
        private readonly IProductSettingService _productSettingService;

        public AppSettingsService(IProductSettingService productSettingService)
        {
            _productSettingService = productSettingService;
        }

        public async Task<AppSettings> GetAsync()
        {
            return new AppSettings
            {
                Product = (await _productSettingService.GetAsync().ConfigureAwait(false)).FirstOrDefault(),
            };
        }
    }
}
