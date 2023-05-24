namespace AppService.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Services;

    [ApiController]
    [Route("[controller]")]
    public class ProductSettingController : BaseController<ProductSetting>
    {
        public ProductSettingController(IProductSettingService productSettingService)
            : base(productSettingService)
        {
        }
    }
}
