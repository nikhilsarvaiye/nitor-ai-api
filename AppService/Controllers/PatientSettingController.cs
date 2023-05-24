namespace AppService.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Services;

    [ApiController]
    [Route("[controller]")]
    public class PatientSettingController : BaseController<PatientSetting>
    {
        public PatientSettingController(IPatientSettingService patientSettingService)
            : base(patientSettingService)
        {
        }
    }
}
