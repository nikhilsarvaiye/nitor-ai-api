namespace Repositories
{
    using Configuration.Options;
    using Models;

    public class TrackerRepository : BaseRepository<TrackerRequest>, ITrackerRepository
    {
        public TrackerRepository(IDbOptions dbOptions)
            : base(dbOptions, "TrackerRequest")
        {
        }
    }
}
