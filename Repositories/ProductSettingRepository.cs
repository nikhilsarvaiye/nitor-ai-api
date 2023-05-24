namespace Repositories
{
    using Configuration.Options;
    using Models;
    
    public class ProductSettingRepository : BaseSettingRepository<ProductSetting>, IProductSettingRepository
    {
        public ProductSettingRepository(IDbOptions dbOptions)
               : base(dbOptions)
        {
        }
    }
}
