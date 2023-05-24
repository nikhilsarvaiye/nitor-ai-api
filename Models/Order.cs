namespace Models
{
    using MongoDB.Bson.Serialization.Attributes;
    using System;
    using System.Collections.Generic;

    public class Order : BaseModel
    {
        public long InvoiceId { get; set; }

        [BsonIgnoreIfNull]
        public OrderStatus Status { get; set; } = OrderStatus.Ordered;

        [BsonIgnoreIfNull]
        public PaymentType PaymentType { get; set; } = PaymentType.Cash;

        [BsonIgnoreIfNull]
        public decimal TaxRate { get; set; }

        [BsonIgnoreIfNull]
        public decimal SubTotalAmount { get; set; }

        [BsonIgnoreIfNull]
        public decimal TaxAmount { get; set; }

        [BsonIgnoreIfNull]
        public decimal TotalAmount { get; set; }

        [BsonIgnoreIfNull]
        public DateTime? DueDate { get; set; }

        [BsonIgnoreIfNull]
        public DateTime? InvoiceDate { get; set; }

        [BsonIgnoreIfNull]
        public string CustomerId { get; set; }

        [BsonIgnore]
        public User Customer { get; set; }

        [BsonIgnoreIfNull]
        public List<OrderProduct> Products { get; set; } = new List<OrderProduct>();

        [BsonIgnoreIfNull]
        public List<OrderHistory> History { get; set; }
    }
}
