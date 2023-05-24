namespace Services
{
    using Configuration.Options;
    using Microsoft.Extensions.DependencyInjection;
    using Models;
    using Repositories;

    public static class ConfigurationExtension
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services, AppOptions appOptions)
        {
            services.ConfigureRepositories();

            if (true || appOptions.Cache)
            {
                services.AddScoped<ICacheService<Product>, CacheService<Product>>();
                services.AddScoped<ICacheService<ProductVariant>, CacheService<ProductVariant>>();
                services.AddScoped<ICacheService<Store>, CacheService<Store>>();
                services.AddScoped<ICacheService<Inventory>, CacheService<Inventory>>();
                services.AddScoped<ICacheService<Order>, CacheService<Order>>();
                services.AddScoped<ICacheService<Promotion>, CacheService<Promotion>>();
                services.AddScoped<ICacheService<User>, CacheService<User>>();
                services.AddScoped<ICacheService<TrackerRequest>, CacheService<TrackerRequest>>();
                services.AddScoped<ICacheService<Notification>, CacheService<Notification>>();
                services.AddScoped<ICacheService<UserNotification>, CacheService<UserNotification>>();
                services.AddScoped<ICacheService<ProductSetting>, CacheService<ProductSetting>>();
                services.AddScoped<ICacheService<OrderSetting>, CacheService<OrderSetting>>();
                // services.AddScoped<ICacheService<AppSettings>, CacheService<AppSettings>>();
            }

            services.AddScoped<IAppSettingsService, AppSettingsService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IProductVariantService, ProductVariantService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITrackerService, TrackerService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IUserNotificationService, UserNotificationService>();
            services.AddScoped<IProductSettingService, ProductSettingService>();
            services.AddScoped<IExcelService, ExcelService>();

            return services;
        }
    }
}
