namespace Models
{
    using Newtonsoft.Json;

    public class TrackerRequest : BaseModel
    {
        public string UserId { get; set; }

        public TrackerRequestType Type { get; set; }

        public string MetaDeta1 { get; set; }

        public string MetaDeta2 { get; set; }

        public string MetaDeta3 { get; set; }

        public string Content { get; set; }

        public TrackerRequestStatus Status { get; set; }

        public TrackerRequestResultType? Result { get; set; }

        public string ResultDetails { get; set; }

        public int TotalSteps { get; set; } = 1;

        public int CurrentStep { get; set; } = 1;

        public string CurrentStepDescription { get; set; }

        public TrackerRequest()
        {
            Status = TrackerRequestStatus.Pending;
        }

        public void PartiallySucceeded(object resultDetails = null)
        {
            Status = TrackerRequestStatus.Completed;
            Result = TrackerRequestResultType.Partial;
            ResultDetails = resultDetails != null ? JsonConvert.SerializeObject(resultDetails) : null;
        }

        public void Succeeded(object resultDetails = null)
        {
            Status = TrackerRequestStatus.Completed;
            CurrentStep = TotalSteps;
            Result = TrackerRequestResultType.Success;
            ResultDetails = resultDetails != null ? JsonConvert.SerializeObject(resultDetails) : null;
        }

        public void Failed(object errorDetails = null)
        {
            Status = TrackerRequestStatus.Completed;
            Result = TrackerRequestResultType.Failed;
            ResultDetails = errorDetails != null ? JsonConvert.SerializeObject(errorDetails) : null;
        }
    }
}
