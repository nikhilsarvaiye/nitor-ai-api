namespace Services
{
    using Configuration.Options;
    using DotnetStandardQueryBuilder.Core;
    using Microsoft.AspNetCore.Http;
    using Models;
    using Repositories;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public abstract class BaseService<T>
        where T : BaseModel, new()
    {
        protected readonly IAppOptions AppOptions;

        protected readonly ICacheService<T> CacheService;

        protected readonly IRepository<T> Repository;

        protected BaseService(IAppOptions appOptions, ICacheService<T> cacheService, IRepository<T> repository)
        {
            AppOptions = appOptions ?? throw new ArgumentException(nameof(appOptions));
            Repository = repository ?? throw new ArgumentException(nameof(repository));
            CacheService = cacheService ?? throw new ArgumentException(nameof(cacheService));
        }

        public virtual async Task<List<T>> BulkCreateAsync(List<T> tList)
        {
            await OnBulkCreating(tList).ConfigureAwait(false);

            return await Repository.BulkCreateAsync(tList).ConfigureAwait(false);
        }

        public virtual async Task<List<string>> BulkRemoveAsync(List<string> ids)
        {
            return await Repository.BulkRemoveAsync(ids).ConfigureAwait(false);
        }

        public virtual async Task<List<string>> BulkRemoveAsync(IFilter filter)
        {
            return await Repository.BulkRemoveAsync(filter).ConfigureAwait(false);
        }

        public virtual async Task<List<string>> BulkUpdateAsync(List<T> tList)
        {
            await OnBulkUpdating(tList).ConfigureAwait(false);

            return await Repository.BulkUpdateAsync(tList).ConfigureAwait(false);
        }

        public virtual async Task<T> CreateAsync(T t)
        {
            await OnCreating(t).ConfigureAwait(false);

            return await Repository.CreateAsync(t).ConfigureAwait(false);
        }

        public virtual async Task<List<T>> GetAsync(IRequest request = null)
        {
            return await Repository.GetAsync(request).ConfigureAwait(false);
        }

        public virtual async Task<T> GetAsync(string id)
        {
            return await Repository.GetAsync(id).ConfigureAwait(false);
        }

        public virtual async Task<List<T>> GetAsync(List<string> ids)
        {
            return await Repository.GetAsync(ids).ConfigureAwait(false);
        }

        public long GetNextSequence()
        {
            return Repository.GetNextSequence();
        }

        public virtual async Task<T> GetOrThrowAsync(string id)
        {
            var item = await GetAsync(id).ConfigureAwait(false);
            if (item == null)
            {
                throw new Exception("Resource not found with Id " + id);
            }
            return item;
        }

        public virtual async Task<IResponse<T>> PaginateAsync(IRequest request)
        {
            return await Repository.PaginateAsync(request).ConfigureAwait(false);
        }

        public virtual async Task<long> RemoveAsync(string id)
        {
            await OnDeleting(id).ConfigureAwait(false);

            return await Repository.RemoveAsync(id).ConfigureAwait(false);
        }

        public virtual async Task<string> UpdateAsync(string id, T t)
        {
            await OnUpdating(t).ConfigureAwait(false);

            return await Repository.UpdateAsync(id, t).ConfigureAwait(false);
        }

        public async Task<List<T>> UploadAsync(IFormFile file)
        {
            var excelService = new ExcelService();
            
            var items = excelService.Import<T>(file);

            var output = new List<T>();

            foreach (var item in items)
            {
                var outputItem = await CreateAsync(item).ConfigureAwait(false);
                output.Add(outputItem);
            }
            
            return output;
        }

        public async Task<List<File>> UploadFilesAsync(List<UploadFile> uploadFiles)
        {
            return await Repository.UploadFilesAsync(uploadFiles).ConfigureAwait(false);
        }

        public async Task<List<UploadFile>> GetFilesAsync(List<string> ids)
        {
            return await Repository.GetFilesAsync(ids).ConfigureAwait(false);
        }

        public async Task<UploadFile> DownloadUploadTemplateAsync()
        {
            var fileName = $"{typeof(T).Name}UploadTemplate.xlsx";

            var templatePath = $@"{System.IO.Directory.GetCurrentDirectory()}\templates\{fileName}";

            var bytes = System.IO.File.ReadAllBytes(templatePath);

            return await Task.FromResult(new UploadFile
            {
                FileName = fileName,
                Bytes = bytes,
                Type = MimeTypes.GetMimeType(fileName),
                Base64 = Convert.ToBase64String(bytes),
            }).ConfigureAwait(false);
        }

        public async Task DeleteFilesAsync(List<string> ids)
        {
            await Repository.DeleteFilesAsync(ids).ConfigureAwait(false);
        }

        public virtual async Task<DownloadFile> ExportAsync(IRequest request = null)
        {
            var items = await GetAsync(request).ConfigureAwait(false);

            var excelService = new ExcelService();

            return excelService.Export(items);
        }

        protected virtual async Task OnBulkCreating(List<T> tList)
        {
            foreach (var item in tList)
            {
                await OnCreating(item).ConfigureAwait(false);
            }
        }

        protected virtual async Task OnBulkUpdating(List<T> tList)
        {
            foreach (var item in tList)
            {
                await OnUpdating(item).ConfigureAwait(false);
            }
        }

        protected abstract Task<T> OnCreating(T t);

        protected virtual async Task OnDeleting(string id)
        {
            await Task.CompletedTask.ConfigureAwait(false);
        }

        protected virtual async Task<T> OnUpdating(T t)
        {
            return await Task.FromResult(t);
        }
    }
}
