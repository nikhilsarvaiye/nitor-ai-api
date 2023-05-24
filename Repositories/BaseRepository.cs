namespace Repositories
{
    using Configuration.Options;
    using Models;
    using MongoDB.Driver;
    using MongoDB.Driver.GridFS;
    using DotnetStandardQueryBuilder.Core;
    using DotnetStandardQueryBuilder.Mongo.Extensions;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System;
    using Common;
    using MongoDB.Bson;
    using System.IO;

    public class BaseRepository<T>
        where T : BaseModel
    {
        private readonly IMongoDatabase _database;

        private readonly IMongoCollection<T> _collection;

        private readonly string SequenceCollectionName = "sequences";

        private readonly object _sequenceLock = new object();

        private const string _idColumnName = "_id";

        protected bool UseSequence { get; set; }

        public BaseRepository(IDbOptions dbOptions, string collectionName)
        {
            var client = new MongoClient(dbOptions.ConnectionString);
            _database = client.GetDatabase(dbOptions.DatabaseName) as MongoDatabaseBase;
            _collection = _database.GetCollection<T>(collectionName);
        }

        public async Task<List<T>> BulkCreateAsync(List<T> tList)
        {
            foreach (var item in tList)
            {
                item.CreatedDateTime = DateTime.Now;
                item.UpdateDateTime = DateTime.Now;
            }

            await _collection.InsertManyAsync(tList).ConfigureAwait(false);

            return tList;
        }

        public async Task<List<string>> BulkRemoveAsync(List<string> ids)
        {
            var writes = new List<WriteModel<T>>();
            var filterDefinition = Builders<T>.Filter;

            foreach (var id in ids)
            {
                var filter = filterDefinition.Where(x => x.Id == id);
                writes.Add(new DeleteOneModel<T>(filter));
            }

            var result = await _collection.BulkWriteAsync(writes).ConfigureAwait(false);

            return result.Upserts.Select(x => x.Id.ToString()).ToList();
        }

        public async Task<List<string>> BulkRemoveAsync(IFilter filter)
        {
            var writes = new List<WriteModel<T>>();
            var filterDefinition = filter.ToFilterDefinition<T>();

            writes.Add(new DeleteManyModel<T>(filterDefinition));

            var result = await _collection.BulkWriteAsync(writes).ConfigureAwait(false);

            return result.Upserts.Select(x => x.Id.ToString()).ToList();
        }

        public async Task<List<string>> BulkUpdateAsync(List<T> tList)
        {
            foreach (var item in tList)
            {
                item.UpdateDateTime = DateTime.Now;
            }

            var ids = tList.Select(x => x.Id);

            var writes = new List<WriteModel<T>>();
            var filterDefinition = Builders<T>.Filter;

            foreach (var t in tList)
            {
                var filter = filterDefinition.Where(x => x.Id == t.Id);
                writes.Add(new ReplaceOneModel<T>(filter, t));
            }

            var result = await _collection.BulkWriteAsync(writes).ConfigureAwait(false);

            return result.Upserts.Select(x => x.Id.ToString()).ToList();
        }

        public async Task<List<T>> GetAsync(IRequest request = null)
        {
            var query = _collection.Query(request);

            return await Task.FromResult(query.ToList()).ConfigureAwait(false);
        }

        public async Task<T> GetAsync(string id) => (await _collection.FindAsync(x => x.Id == id).ConfigureAwait(false)).FirstOrDefault();

        public async Task<List<T>> GetAsync(List<string> ids) => (await _collection.FindAsync(x => ids.Contains(x.Id)).ConfigureAwait(false)).ToList();

        public long GetNextSequence()
        {
            lock (_sequenceLock)
            {
                var filter = Builders<Sequence>.Filter.Eq(_idColumnName, typeof(T).Name);
                var update = Builders<Sequence>.Update.Inc(x => x.SequenceId, 1);

                var result = _database.GetCollection<Sequence>(SequenceCollectionName).FindOneAndUpdate(filter, update, new FindOneAndUpdateOptions<Sequence, Sequence>
                {
                    IsUpsert = true
                });
                
                return result?.SequenceId + 1 ?? 1;
            }
        }

        public async Task<IResponse<T>> PaginateAsync(IRequest request)
        {
            var query = _collection.Query(request);

            return new Response<T>
            {
                Count = await _collection.QueryCount(request).CountDocumentsAsync().ConfigureAwait(false),
                Items = query.ToList()
            };
        }

        public async Task<T> CreateAsync(T t)
        {
            t.CreatedDateTime = DateTime.Now;
            t.UpdateDateTime = DateTime.Now;

            await _collection.InsertOneAsync(t).ConfigureAwait(false);

            return t;
        }

        public async Task<long> RemoveAsync(string id)
        {
            var result = await _collection.DeleteOneAsync(x => x.Id == id).ConfigureAwait(false);

            return result.IsAcknowledged ? result.DeletedCount : 0;
        }

        public async Task<string> UpdateAsync(string id, T t)
        {
            t.UpdateDateTime = DateTime.Now;

            // var updateDefinition = Builders<T>.Update.Set("", t.Id);

            // foreach (var property in t.GetType().GetProperties())
            // {
            //     updateDefinition.Set(property.Name, property.GetValue(t));
            // }

            // var result = await _collection.UpdateOneAsync(x => x.Id == id, updateDefinition).ConfigureAwait(false);

            var result = await _collection.ReplaceOneAsync(x => x.Id == id, t).ConfigureAwait(false);

            return result.MatchedCount == 1 ? id : string.Empty;
        }

        public async Task<List<Models.File>> UploadFilesAsync(List<UploadFile> uploadFiles)
        {
            var bucket = new GridFSBucket(_database);

            var tasks = new List<Task<ObjectId>>();
            var streams = new List<Stream>();

            foreach (var uploadFile in uploadFiles)
            {
                var stream = uploadFile.File.OpenReadStream();
                tasks.Add(bucket.UploadFromStreamAsync(uploadFile.File.FileName, stream, new GridFSUploadOptions
                {
                    Metadata = uploadFile.Metadata?.ToBsonDocument()
                }));
                streams.Add(stream);
            }

            var objectIds = (await Task.WhenAll(tasks).ConfigureAwait(false)).ToList();

            foreach (var stream in streams)
            {
                stream?.Dispose();
            }

            return objectIds.ConvertAll(x => new Models.File
            {
                Id = x.ToString(),
                FileName = uploadFiles[objectIds.IndexOf(x)].File.FileName,
            });
        }


        public async Task DeleteFilesAsync(List<string> ids)
        {
            var bucket = new GridFSBucket(_database);

            var tasks = new List<Task>();

            foreach (var objectId in ids.ConvertAll(id => ObjectId.Parse(id)))
            {
                tasks.Add(bucket.DeleteAsync(objectId));
            }

            await Task.WhenAll(tasks).ConfigureAwait(false);
        }

        public async Task<List<UploadFile>> GetFilesAsync(List<string> ids)
        {
            var bucket = new GridFSBucket(_database);

            var objectIds = ids.ConvertAll(id => ObjectId.Parse(id));

            var gridFSFilesInfo = (await bucket.FindAsync(Builders<GridFSFileInfo>.Filter.In(_idColumnName, objectIds)).ConfigureAwait(false)).ToList();

            return (await Task.WhenAll(objectIds.Select(async objectId =>
            {
                var gridFSFileInfo = gridFSFilesInfo.Find(x => x.Id == objectId);

                var bytes = await bucket.DownloadAsBytesAsync(BsonValue.Create(objectId)).ConfigureAwait(false);

                return new UploadFile
                {
                    Id = objectId.ToString(),
                    FileName = gridFSFileInfo.Filename,
                    Bytes = bytes,
                    Type = MimeTypes.GetMimeType(gridFSFileInfo.Filename),
                    Base64 = Convert.ToBase64String(bytes),
                    Metadata = gridFSFileInfo.Metadata?.ToDictionary()
                };
            })).ConfigureAwait(false)).ToList();
        }
    }
}
