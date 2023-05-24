namespace Repositories
{
    using Configuration.Options;
    using Models;
    
    public class PatientSettingRepository : BaseSettingRepository<PatientSetting>, IPatientSettingRepository
    {
        public PatientSettingRepository(IDbOptions dbOptions)
               : base(dbOptions)
        {
        }
    }
}
