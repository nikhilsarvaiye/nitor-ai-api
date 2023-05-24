namespace Models
{
    using MongoDB.Bson.Serialization.Attributes;
    using System.Collections.Generic;
    
    public class ProductVariant : BaseModel
    {
        public string ProductId { get; set; }

        public string Name { get; set; }

        public ProductVariantType Type { get; set; } = ProductVariantType.Default;

        public decimal? VariantPrice { get; set; }

        public decimal Price { get; set; }

        [BsonExtraElements]
        public Dictionary<string, object> Values { get; set; }
        
        public virtual Product Product { get; set; }
    }
}
