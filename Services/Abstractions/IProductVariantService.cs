namespace Services
{
    using Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IProductVariantService : IService<ProductVariant>
    {
        Task<List<Product>> UpdateProducts(List<Product> products);
    }
}
