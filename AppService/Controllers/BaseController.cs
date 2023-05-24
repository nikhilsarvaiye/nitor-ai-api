namespace AppService.Controllers
{
    using Common;
    using DotnetStandardQueryBuilder.Core;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using MongoDB.Bson;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Services;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public abstract class BaseController<T> : ControllerBase
        where T : BaseModel, new()
    {
        protected readonly IService<T> Service;

        protected BaseController(IService<T> service)
        {
            Service = service ?? throw new ArgumentNullException(nameof(service));
        }

        // [Authorize(Roles = "Admin")]
        [HttpGet]
        public virtual async Task<dynamic> GetAsync(string? id = null)
        {
            if (!string.IsNullOrEmpty(id))
            {
                return new List<T>() { await Service.GetAsync(id).ConfigureAwait(false) }.FirstOrDefault();
            }

            var request = GetRequestFromQueryString();

            if (request.Count)
            {
                return await Service.PaginateAsync(request).ConfigureAwait(false);
            }

            return await Service.GetAsync(request).ConfigureAwait(false);
        }

        [HttpGet("paginate")]
        public virtual async Task<IResponse<T>> PaginateAsync(int? pageSize = null, int page = 1)
        {
            return await Service.PaginateAsync(new Request { Count = true, Page = 1, PageSize = pageSize }).ConfigureAwait(false);
        }

        [HttpPost]
        public virtual async Task<T> CreateAsync(T skill)
        {
            return await Service.CreateAsync(skill).ConfigureAwait(false);
        }

        [HttpPost("createorupdate")]
        public virtual async Task CreateOrUpdateAsync(T t)
        {
            if (!string.IsNullOrEmpty(t.Id))
            {
                await UpdateAsync(t.Id, t).ConfigureAwait(false);
                return;
            }

            var request = new DotnetStandardQueryBuilder.OData.UriParser(new DotnetStandardQueryBuilder.OData.UriParserSettings()).Parse<T>(Request.QueryString.ToString());

            request.PageSize = 1;

            var items = await Service.GetAsync(request).ConfigureAwait(false);

            var item = items.FirstOrDefault();

            if (item != null)
            {
                await UpdateAsync(item.Id, item).ConfigureAwait(false);
            }

            await CreateAsync(t).ConfigureAwait(false);
        }

        [HttpPut]
        public virtual async Task<string> UpdateAsync(string id, T t)
        {
            if (string.IsNullOrEmpty(Convert.ToString(id)))
            {
                throw new ArgumentNullException(nameof(id));
            }

            return await Service.UpdateAsync(id, t).ConfigureAwait(false);
        }

        [HttpDelete]
        public virtual async Task DeleteAsync(string id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(id)))
            {
                throw new ArgumentNullException(nameof(id));
            }

            await Service.RemoveAsync(id).ConfigureAwait(false);
        }



        [HttpDelete("bulkdelete")]
        public async Task BulkDeleteAsync()
        {
            var request = GetRequestFromQueryString();

            var filter = request.Filter as Filter;

            var ids = (filter?.Value as IEnumerable)?.ToList().ConvertAll(x => Convert.ToString(x));

            if (ids == null || ids.Count == 0)
            {
                throw new ArgumentNullException("Kindly provide ids as odata query IN filter");
            }

            await Service.BulkRemoveAsync(ids).ConfigureAwait(false);
        }

        [HttpGet("export")]
        public async Task<DownloadFile> DownloadFile()
        {
            var request = new DotnetStandardQueryBuilder.OData.UriParser(new DotnetStandardQueryBuilder.OData.UriParserSettings()).Parse<T>(Request.QueryString.ToString());

            request.Count = false;

            return await Service.ExportAsync(request).ConfigureAwait(false);
        }

        [HttpPost("upload")]
        public async Task<List<T>> UploadAsync([FromForm] IFormFile file)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            return await Service.UploadAsync(file).ConfigureAwait(false);
        }

        [HttpPost("file")]
        public async Task<List<File>> UploadFilesAsync([FromForm] UploadFileRequest uploadFileRequest)
        {
            var uploadFiles = new List<UploadFile>();
            foreach (var file in uploadFileRequest.Files)
            {
                var metadataValue = uploadFileRequest.MetadataValues[uploadFileRequest.Files.IndexOf(file)];
                uploadFiles.Add(new UploadFile
                {
                    File = file,
                    Metadata = !string.IsNullOrEmpty(metadataValue) ? JsonConvert.DeserializeObject<Dictionary<string, object>>(metadataValue) : null
                });
            }

            return await Service.UploadFilesAsync(uploadFiles).ConfigureAwait(false);
        }

        [HttpGet("file")]
        public async Task<List<UploadFile>> GetFilesAsync()
        {
            var request = GetRequestFromQueryString();

            var filter = request.Filter as Filter;

            var ids = (filter?.Value as IEnumerable)?.ToList().ConvertAll(x => Convert.ToString(x));

            if (ids == null || ids.Count == 0)
            {
                throw new ArgumentNullException("Kindly provide ids as odata query IN filter");
            }

            return await Service.GetFilesAsync(ids).ConfigureAwait(false);
        }

        [HttpGet("downloaduploadtemplate")]
        public async Task<UploadFile> DownloadUploadTemplateAsync()
        {
            return await Service.DownloadUploadTemplateAsync().ConfigureAwait(false);
        }

        [HttpDelete("file")]
        public async Task DeleteFilesAsync()
        {
            var request = GetRequestFromQueryString();

            var filter = request.Filter as Filter;

            var ids = (filter?.Value as IEnumerable)?.ToList().ConvertAll(x => Convert.ToString(x));

            if (ids == null || ids.Count == 0)
            {
                throw new ArgumentNullException("Kindly provide ids as odata query IN filter");
            }

            await Service.DeleteFilesAsync(ids).ConfigureAwait(false);
        }

        protected IRequest GetRequestFromQueryString()
        {
            return new DotnetStandardQueryBuilder.OData.UriParser(new DotnetStandardQueryBuilder.OData.UriParserSettings()).Parse<T>(Request.QueryString.ToString());
        }
    }
}
