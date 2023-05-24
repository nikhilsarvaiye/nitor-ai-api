namespace Services
{
    using CacheManager.Core;
    using ClosedXML.Excel;
    using Configuration.Options;
    using DotnetStandardQueryBuilder.Core;
    using DotnetStandardQueryBuilder.MemoryList.Extensions;
    using Microsoft.AspNetCore.Http;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class CacheService<T> : ICacheService<T>
        where T : BaseModel
    {
        private readonly IAppOptions _appOptions;

        private readonly string _cacheKey = typeof(T).Name;

        public CacheService(IAppOptions appOptions)
        {
            _appOptions = appOptions ?? throw new ArgumentException(nameof(appOptions));
            // _cacheManager = cacheManager;
        }

        public List<string> Keys
        {
            get
            {
                //var keys = _cacheManager.Get<string[]>(_cacheKey);

                //if (keys == null)
                //{
                //    keys = new string[] { };
                //    _cacheManager.Add(_cacheKey, keys);
                //}

                // return keys.ToList();

                return new List<string>();
            }
        }

        public async Task<List<T>> BulkCreateAsync(List<T> tList)
        {
            foreach (var t in tList)
            {
                await CreateAsync(t);
            }
            return await Task.FromResult(tList);
        }

        public async Task<List<string>> BulkRemoveAsync(List<string> ids)
        {
            foreach (var id in ids)
            {
                await RemoveAsync(id);
            }
            return ids;
        }

        public Task<List<string>> BulkRemoveAsync(IFilter filter)
        {
            throw new NotImplementedException();
        }

        public async Task<List<string>> BulkUpdateAsync(List<T> tList)
        {
            foreach (var t in tList)
            {
                await UpdateAsync(t.Id, t);
            }
            return tList.Select(x => x.Id).ToList();
        }

        public async Task<T> CreateAsync(T t)
        {
            string newId = t.Id;
            //_cacheManager.Update(_cacheKey, obj =>
            //{
            //    var keys = (obj as string[]).ToList();

            //    // TODO: change id for distributed cache
            //    // newId = !keys.Any() ? 1 : keys.Max() + 1;
            //    keys.Add(newId);
            //    return keys.ToArray();
            //});

            //t.Id = newId;
            //_cacheManager.Add(newId, t);

            return await Task.FromResult(t);
        }

        public Task<List<UploadFile>> GetFilesAsync(List<string> ids)
        {
            throw new NotImplementedException();
        }

        public Task<DownloadFile> ExportAsync(IRequest request = null)
        {
            throw new NotImplementedException();
        }

        public async Task<T> GetAsync(string key)
        {
            // return await Task.FromResult(_cacheManager.Get<T>(key));

            return null;
        }

        public async Task<List<T>> GetAsync(IRequest request = null)
        {
            var items = new List<T>();
            foreach (var key in Keys)
            {
                items.Add(await GetAsync(key));
            }
            return request == null ? items : items.Query(request);
        }

        public virtual async Task<List<T>> GetAsync(List<string> ids)
        {
            var items = new List<T>();
            foreach (var key in Keys.Where(k => ids.Contains(k)))
            {
                items.Add(await GetAsync(key));
            }
            return items;
        }

        public async Task<IResponse<T>> PaginateAsync(IRequest request)
        {
            var items = new List<T>();
            foreach (var key in Keys)
            {
                items.Add(await GetAsync(key));
            }
            return new Response<T>
            {
                Count = items.QueryCount(request),
                Items = items.Query(request)
            };
        }

        public async Task<long> RemoveAsync(string key)
        {
            //_cacheManager.Remove(key);
            //_cacheManager.Update(_cacheKey, obj =>
            //{
            //    var keys = (obj as string[]).ToList();
            //    keys.Remove(key);
            //    return keys.ToArray();
            //});
            return await Task.FromResult(1);
        }

        public async Task<string> UpdateAsync(string key, T t)
        {
            // _cacheManager.Put(key, t);

            return await Task.FromResult(key);
        }

        public Task<List<File>> UploadFilesAsync(List<UploadFile> uploadFiles)
        {
            throw new NotImplementedException();
        }

        public Task DeleteFilesAsync(List<string> ids)
        {
            throw new NotImplementedException();
        }

        public long GetNextSequence()
        {
            throw new NotImplementedException();
        }

        public Task<UploadFile> DownloadUploadTemplateAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<T>> UploadAsync(IFormFile file)
        {
            throw new NotImplementedException();
        }
    }
}
