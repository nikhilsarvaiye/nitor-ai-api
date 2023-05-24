namespace AppService.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Services;

    [ApiController]
    [Route("[controller]")]
    public class ProductVariantController : BaseController<ProductVariant>
    {
        public ProductVariantController(IProductVariantService productVariantService)
            : base(productVariantService)
        {
        }
    }
}
