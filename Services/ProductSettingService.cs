namespace Services
{
    using Configuration.Options;
    using Models;
    using Repositories;

    public class ProductSettingService : BaseSettingService<ProductSetting>, IProductSettingService
    {
        public ProductSettingService(IAppOptions appOptions, ICacheService<ProductSetting> cacheService, IProductSettingRepository productSettingRepository)
            : base(SettingType.Product, appOptions, cacheService, productSettingRepository)
        {
        }
    }
}
