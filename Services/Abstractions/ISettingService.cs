namespace Services
{
    using Models;
    
    public interface ISettingService<T> : IService<T>
        where T : Setting
    {
    }
}
