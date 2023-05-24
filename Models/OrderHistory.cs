namespace Models
{
    using MongoDB.Bson.Serialization.Attributes;
    using System;
    
    public class OrderHistory
    {
        public OrderStatus Status { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreatedDateTime { get; set; } = DateTime.Now;
    }
}
