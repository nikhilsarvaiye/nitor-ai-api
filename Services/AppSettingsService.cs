namespace Services
{
    using Models;
    using System.Linq;
    using System.Threading.Tasks;

    public class AppSettingsService : IAppSettingsService
    {
        private readonly IPatientSettingService _patientSettingService;

        public AppSettingsService(IPatientSettingService patientSettingService)
        {
            _patientSettingService = patientSettingService;
        }

        public async Task<AppSettings> GetAsync()
        {
            return new AppSettings
            {
                Patient = (await _patientSettingService.GetAsync().ConfigureAwait(false)).FirstOrDefault(),
            };
        }
    }
}
