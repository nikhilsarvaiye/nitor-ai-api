namespace Repositories
{
    using Configuration.Options;
    using Models;
    using Repositories;

    public class ProductVariantRepository : BaseRepository<ProductVariant>, IProductVariantRepository
    {
        public ProductVariantRepository(IDbOptions dbOptions)
            : base(dbOptions, "productVariants")
        {
        }
    }
}
