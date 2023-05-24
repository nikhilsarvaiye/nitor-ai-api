namespace Repositories
{
    using Configuration.Options;
    using Models;
    using Repositories;

    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(IDbOptions dbOptions)
            : base(dbOptions, "users")
        {
        }
    }
}
