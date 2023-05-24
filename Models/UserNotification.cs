namespace Models
{
	using System;

    public class UserNotification : BaseModel
	{
		public string UserId { get; set; }

		public string NotificationId { get; set; }

		public bool IsRead { get; set; }

		public DateTime? ReadDateTime { get; set; }
	}
}
