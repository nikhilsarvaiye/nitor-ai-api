namespace Services
{
    using Models;
    using System.Threading.Tasks;

    public interface IAppSettingsService
    {
        Task<AppSettings> GetAsync();
    }
}
