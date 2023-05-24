namespace Repositories
{
    using Configuration.Options;
    using Models;
    using Repositories;

    public class PatientRepository : BaseRepository<Patient>, IPatientRepository
    {
        public PatientRepository(IDbOptions dbOptions)
            : base(dbOptions, "patients")
        {
        }
    }
}
