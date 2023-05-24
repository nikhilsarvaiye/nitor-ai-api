﻿namespace Models
{
    public class CreateTrackerRequest
    {
        public string UserId { get; set; }

        public TrackerRequestType Type { get; set; }

        public string MetaDeta1 { get; set; }

        public string MetaDeta2 { get; set; }

        public string MetaDeta3 { get; set; }

        public object Content { get; set; }

        public int TotalSteps { get; set; } = 1;

        public int CurrentStep { get; set; } = 1;

        public string CurrentStepDescription { get; set; }
    }
}
