namespace Repositories
{
    using Configuration.Options;
    using Models;

    public class UserNotificationRepository : BaseRepository<UserNotification>, IUserNotificationRepository
    {
        public UserNotificationRepository(IDbOptions dbOptions)
            : base(dbOptions, "UserNotification")
        {
        }
    }
}
