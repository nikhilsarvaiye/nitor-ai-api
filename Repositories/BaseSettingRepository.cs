namespace Repositories
{
    using Configuration.Options;
    using Models;
    using Repositories;

    public abstract class BaseSettingRepository<T> : BaseRepository<T>
        where T: Setting, new()
    {
        public BaseSettingRepository(IDbOptions dbOptions)
            : base(dbOptions, "settings")
        {
        }
    }
}
