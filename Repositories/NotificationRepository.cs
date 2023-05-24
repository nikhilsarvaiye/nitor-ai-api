namespace Repositories
{
    using Configuration.Options;
    using Models;

    public class NotificationRepository : BaseRepository<Notification>, INotificationRepository
    {
        public NotificationRepository(IDbOptions dbOptions)
            : base(dbOptions, "Notification")
        {
        }
    }
}
