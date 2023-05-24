namespace Models
{
    using MongoDB.Bson.Serialization.Attributes;
    using System.Collections.Generic;
    
    public class Product : BaseModel
    {
        public string Sku { get; set; }

        public string Name { get; set; }

        [BsonIgnoreIfNull]
        public string Description { get; set; }

        [BsonIgnoreIfNull]
        public string MajorDepartment { get; set; }

        [BsonIgnoreIfNull]
        public string Department { get; set; }

        public string Category { get; set; }

        [BsonIgnoreIfNull]
        public string SubCategory { get; set; }

        [BsonIgnoreIfNull]
        public string Type { get; set; }

        [BsonIgnoreIfNull]
        public string Brand { get; set; }

        [BsonIgnoreIfNull]
        public string Manufacturer { get; set; }

        [BsonIgnoreIfNull]
        public List<string> Tags { get; set; }

        [BsonIgnoreIfNull]
        public string Thumbnail { get; set; }

        public decimal Price { get; set; }

        [BsonIgnoreIfNull]
        public decimal? ComparePrice { get; set; }

        [BsonIgnoreIfNull]
        public decimal Cost { get; set; }

        [BsonIgnoreIfNull]
        public bool ChargeTax { get; set; }

        [BsonIgnoreIfNull]
        public string Barcode { get; set; }

        [BsonIgnoreIfNull]
        public bool TrackQuantity { get; set; }

        [BsonIgnoreIfNull]
        public short Quantity { get; set; }

        [BsonIgnoreIfNull]
        public bool IsPhysicalProduct { get; set; }

        [BsonIgnoreIfNull]
        public short Weight { get; set; }

        [BsonIgnoreIfNull]
        public string WeightUnit { get; set; }

        [BsonIgnoreIfNull]
        public string Status { get; set; }

        [BsonIgnoreIfNull]
        public string Vendor { get; set; }

        [BsonIgnoreIfNull]
        public string Collections { get; set; }

        [BsonIgnoreIfNull]
        public List<ProductOption> Options { get; set; }

        [BsonIgnoreIfNull]
        public List<ProductVariantNew> Variants { get; set; }

        [BsonIgnore]
        [BsonExtraElements]
        public Dictionary<string, object> Metadata { get; set; } = new Dictionary<string, object>();
    }

    public class ProductOption
    {
        public string Name { get; set; }

        public List<string> Values { get; set; } = new List<string>();
    }

    public class ProductVariantNew
    {
        public string Option { get; set; }

        public string Variant { get; set; }

        public decimal Price { get; set; }

        public short Quantity { get; set; }

        public string Sku { get; set; }
    }
}
