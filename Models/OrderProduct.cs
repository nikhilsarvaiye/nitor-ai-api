namespace Models
{
    public class OrderProduct
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public long Quantity { get; set; }

        public decimal Rate { get; set; }

        public decimal Amount { get; set; }
    }
}
