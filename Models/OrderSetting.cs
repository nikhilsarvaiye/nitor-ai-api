namespace Models
{
    public class OrderSetting : Setting
    {
        public OrderSchema Schema { get; set; }

        public OrderViews Views { get; set; }
    }
}
