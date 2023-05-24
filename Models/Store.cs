namespace Models
{
    public class Store : BaseModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string Address { get; set; }

        public string Area { get; set; }

        public string Landmark { get; set; }

        public string City { get; set; }

        public string PinCode { get; set; }

        public string State { get; set; }

        public string Country { get; set; }
    }
}
