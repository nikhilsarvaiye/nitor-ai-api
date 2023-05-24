namespace Services
{
    using Configuration.Options;
    using Models;
    using Repositories;

    public class PatientSettingService : BaseSettingService<PatientSetting>, IPatientSettingService
    {
        public PatientSettingService(IAppOptions appOptions, ICacheService<PatientSetting> cacheService, IPatientSettingRepository patientSettingRepository)
            : base(SettingType.Patient, appOptions, cacheService, patientSettingRepository)
        {
        }
    }
}
