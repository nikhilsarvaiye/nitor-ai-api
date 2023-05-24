namespace Repositories
{
    using Models;
    
    public interface ISettingRepository<T> : IRepository<T>
        where T : Setting
    {
    }
}
