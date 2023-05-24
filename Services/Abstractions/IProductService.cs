namespace Services
{
    using Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IProductService : IService<Product>
    {
        string VariantsPropertyName { get; }

        List<ProductVariant> ParseVariants(string id, Product product);
    }
}
