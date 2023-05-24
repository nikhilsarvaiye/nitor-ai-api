namespace Models
{
    using MongoDB.Bson;
    using System;
    
    public class DownloadFile
    {
        public string Base64 { get; set; }

        public string Type { get; set; }

        public string FileName { get; set; }

        public BsonDocument Metadata { get; set; }

        public DateTime CreatedDateTime { get; set; }
    }
}
