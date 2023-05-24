namespace AppService.Controllers
{
    using CacheManager.Core;
    using DotnetStandardQueryBuilder.Core;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Services;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class PatientController : BaseController<Patient>
    {
        private readonly IPatientService _patientService;

        private readonly ICacheManager<string> _cache;

        public PatientController(IPatientService patientService)
            : base(patientService)
        {
            _patientService = patientService ?? throw new ArgumentNullException(nameof(patientService));
        }

        [HttpGet]
        public override async Task<dynamic> GetAsync(string? id = null)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var patient = new List<Patient>() { await base.GetAsync(id).ConfigureAwait(false) }.FirstOrDefault();

                return patient;
            }

            var request = GetRequestFromQueryString();

            if (request.Count)
            {
                var patientsResponse = await Service.PaginateAsync(request).ConfigureAwait(false);

                return patientsResponse;
            }

            var patients = await Service.GetAsync(request).ConfigureAwait(false);

            return patients;
        }

        [HttpPost]
        public override async Task<Patient> CreateAsync(Patient patient)
        {
            var createdPatient = await base.CreateAsync(patient).ConfigureAwait(false);

            return createdPatient;
        }

        [HttpPut]
        public override async Task<string> UpdateAsync(string id, Patient patient)
        {
            if (string.IsNullOrEmpty(Convert.ToString(id)))
            {
                throw new ArgumentNullException(nameof(id));
            }

            var updatedId = await base.UpdateAsync(id, patient).ConfigureAwait(false);

            return updatedId;
        }

        [HttpDelete]
        public override async Task DeleteAsync(string id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(id)))
            {
                throw new ArgumentNullException(nameof(id));
            }

            await base.DeleteAsync(id).ConfigureAwait(false);
        }
    }
}
