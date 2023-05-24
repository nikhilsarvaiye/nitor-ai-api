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
                services.AddScoped<ICacheService<Patient>, CacheService<Patient>>();
                services.AddScoped<ICacheService<User>, CacheService<User>>();
                services.AddScoped<ICacheService<TrackerRequest>, CacheService<TrackerRequest>>();
                services.AddScoped<ICacheService<Notification>, CacheService<Notification>>();
                services.AddScoped<ICacheService<UserNotification>, CacheService<UserNotification>>();
                services.AddScoped<ICacheService<PatientSetting>, CacheService<PatientSetting>>();
                // services.AddScoped<ICacheService<AppSettings>, CacheService<AppSettings>>();
            }

            services.AddScoped<IAppSettingsService, AppSettingsService>();
            services.AddScoped<IPatientService, PatientService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITrackerService, TrackerService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IUserNotificationService, UserNotificationService>();
            services.AddScoped<IPatientSettingService, PatientSettingService>();
            services.AddScoped<IExcelService, ExcelService>();

            return services;
        }
    }
}
