﻿namespace Models
{
    public class Notification : BaseModel
    {
		public string RequestId { get; set; }

		public string UserId { get; set; }

		public NotificationMessageType Type { get; set; }

		public NotificationMessageSeverity? Severity { get; set; }

		public string MetaDeta1 { get; set; }

		public string MetaDeta2 { get; set; }

		public string MetaDeta3 { get; set; }

		public string Content { get; set; }

        public Notification()
        {
			Severity = NotificationMessageSeverity.Info;
		}
	}
}
