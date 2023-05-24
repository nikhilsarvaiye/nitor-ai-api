using System.Collections.Generic;

namespace Models
{
    public class CreateNotificationRequest : Notification
    {
        public new object Content { get; set; }

        public List<string> Users { get; set; } = new List<string>();
    }
}
