namespace Repositories
{
    using Configuration.Options;
    using Models;
    using Repositories;

    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(IDbOptions dbOptions)
            : base(dbOptions, "products")
        {
        }
    }
}
