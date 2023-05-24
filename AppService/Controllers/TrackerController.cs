namespace AppService.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Services;
    using System;
    using System.Threading.Tasks;

    [ApiController]
    [Route("[controller]")]
    public class TrackerController : BaseController<TrackerRequest>
    {
        private readonly ITrackerService _trackingService;

        public TrackerController(ITrackerService trackingService)
            : base(trackingService)
        {
            _trackingService = trackingService ?? throw new ArgumentNullException(nameof(trackingService));
        }

        [HttpPost("create")]
        public async Task<TrackerRequest> CreateAsync(CreateTrackerRequest createTrackerRequest)
        {
            return await _trackingService.CreateAsync(createTrackerRequest);
        }

        [HttpPut("step/{id}")]
        public async Task<string> UpdateStepAsync(string id, UpdateTrackerStepRequest updateTrackerRequestStep)
        {
            if (updateTrackerRequestStep == null)
            {
                throw new ArgumentNullException(nameof(updateTrackerRequestStep));
            }

            return await _trackingService.UpdateStepAsync(id, updateTrackerRequestStep);
        }

        [HttpPut("complete/{id}")]
        public async Task<string> CompleteAsync(string id, CompleteTrackerRequest completeTrackerRequest)
        {
            if (completeTrackerRequest == null)
            {
                throw new ArgumentNullException(nameof(completeTrackerRequest));
            }

            return await _trackingService.CompleteAsync(id, completeTrackerRequest);
        }
    }
}
