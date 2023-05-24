namespace Models
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;
    
    public class Sequence
    {
        [BsonId] // Used by Mongo to use this property as _id, name can be anything not "Id"
        [BsonRepresentation(BsonType.ObjectId)] // Used for Mongo-.Net Casting 
        public virtual string Id { get; set; }

        public long SequenceId { get; set; }
    }
}
