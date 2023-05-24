namespace Repositories
{
    using DotnetStandardQueryBuilder.Core;
    using Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IRepository<T>
    {
        Task<List<T>> BulkCreateAsync(List<T> tList);

        Task<List<string>> BulkRemoveAsync(List<string> ids);

        Task<List<string>> BulkRemoveAsync(IFilter filter);

        Task<List<string>> BulkUpdateAsync(List<T> tList);

        Task<T> CreateAsync(T t);

        Task<List<T>> GetAsync(IRequest request = null);

        Task<T> GetAsync(string id);

        Task<List<T>> GetAsync(List<string> ids);

        long GetNextSequence();

        Task<IResponse<T>> PaginateAsync(IRequest request);

        Task<long> RemoveAsync(string id);

        Task<string> UpdateAsync(string id, T t);

        Task<List<File>> UploadFilesAsync(List<UploadFile> uploadFiles);

        Task DeleteFilesAsync(List<string> ids);

        Task<List<UploadFile>> GetFilesAsync(List<string> ids);
    }
}
