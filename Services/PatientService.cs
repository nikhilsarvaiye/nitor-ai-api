namespace Services
{
    using FluentValidation;
    using Models;
    using DotnetStandardQueryBuilder.Core;
    using Repositories;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Validators;
    using Configuration.Options;
    using System.Collections;
    using Common;

    public class PatientService : BaseService<Patient>, IPatientService
    {
        private readonly IExcelService _excelService;

        public PatientService(IAppOptions appOptions, ICacheService<Patient> cacheService, IPatientRepository patientRepository, IExcelService excelService)
            : base(appOptions, cacheService, patientRepository)
        {
            _excelService = excelService ?? throw new ArgumentNullException(nameof(excelService));
        }
        
        protected override async Task<Patient> OnCreating(Patient patient)
        {
            new PatientValidator().ValidateAndThrow(patient);

            return await Task.FromResult(patient).ConfigureAwait(false);
        }
    }
}
