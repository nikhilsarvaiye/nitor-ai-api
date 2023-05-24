namespace SDK
{
    using Models;
    using Rebus.Activation;
    using RestSharp;
    using System;
    using System.Threading.Tasks;

    public class Tracker : Base<TrackerRequest>
    {
        public Tracker(string apiUrl, IHandlerActivator activator)
            : base(apiUrl, "trackerrequest", activator)
        {
        }

        public override Task<TrackerRequest> CreateAsync(TrackerRequest t)
        {
            throw new NotImplementedException($"Kindly use another method CreateAsync method with {nameof(CreateTrackerRequest)} as input");
        }

        public async Task<TrackerRequest> CreateAsync(CreateTrackerRequest createTrackerRequest)
        {
            var response = RestClient.Post<TrackerRequest>(new RestRequest($"{Resource}/create").AddJsonBody(createTrackerRequest));
            if (response.Data != null)
            {
                ServiceBus.Bus.Advanced.SyncBus.Publish(response.Data);
            }
            return await Task.FromResult(response.Data);
        }

        public async Task<string> UpdateStepAsync(string id, UpdateTrackerStepRequest updateTrackerStepRequest)
        {
            return await Task.FromResult(RestClient.Put(new RestRequest($"{Resource}/step/{id}").AddJsonBody(updateTrackerStepRequest)).Content);
        }

        public async Task<string> CompleteAsync(string id, CompleteTrackerRequest completeTrackerRequest)
        {
            return await Task.FromResult(RestClient.Put(new RestRequest($"{Resource}/complete/{id}").AddJsonBody(completeTrackerRequest)).Content);
        }
    }
}
