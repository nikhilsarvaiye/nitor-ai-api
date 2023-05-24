namespace Models
{
    public class ProductSetting : Setting
    {
        public ProductSchema Schema { get; set; }

        public ProductViews Views { get; set; }
    }
}
