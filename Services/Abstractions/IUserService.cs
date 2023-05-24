namespace Services
{
    using Models;
    using System.Threading.Tasks;

    public interface IUserService : IService<User>
    {
        Task<LoggedInUser> LogInAsync(string id, string password);

        Task<LoggedInUser> RegisterAsync(User user);

        Task UpdateThemeAsync(string id, string theme);
    }
}
