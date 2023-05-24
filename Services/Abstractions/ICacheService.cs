namespace Services
{
    using Models;
    using System.Collections.Generic;

    public interface ICacheService<T> : IService<T>
        where T: BaseModel
    {
        List<string> Keys { get; }
    }
}
