namespace Models
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class BaseModel
    {
        // Default recommanded to have BsonType.ObjectId as it supports casting for both ObjectId and any String value if you pass as id
        [Key] // This is for OData Query Model
        [BsonId] // Used by Mongo to use this property as _id, name can be anything not "Id"
        [BsonRepresentation(BsonType.ObjectId)] // Used for Mongo-.Net Casting 
        public virtual string Id { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreatedDateTime { get; set; } = DateTime.Now;

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime UpdateDateTime { get; set; } = DateTime.Now;

        [BsonIgnore]
        public List<UploadFile> UploadFiles { get; set; } = new List<UploadFile>();

        [BsonIgnoreIfNull]
        public List<File> Files { get; set; }

        // BsonExtraElements Documentation 
        // https://mongodb.github.io/mongo-csharp-driver/2.11/examples/mixing_static_and_dynamic/
        // says to use BsonDocument, even it throws exception but this works like you want
        // it stores as part of document in database and retries as well !!!
    }
}
