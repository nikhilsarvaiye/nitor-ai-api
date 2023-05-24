namespace Services
{
    using Models;
    using System.Threading.Tasks;

    public interface ITrackerService : IService<TrackerRequest>
    {
        Task<TrackerRequest> CreateAsync(CreateTrackerRequest createTrackerRequest);

        Task<string> UpdateStepAsync(string id, UpdateTrackerStepRequest updateTrackerStepRequest);

        Task<string> CompleteAsync(string id, CompleteTrackerRequest completeTrackerRequest);
    }
}
