namespace Models
{
    public enum OrderStatus
    {
        Ordered = 10,

        Received = 20,
        
        Picked = 30,

        Shipping = 40,

        Arrived = 50,

        OutForDelivery = 50,

        Delivered = 60,

        Returned = 70,

        Cancelled = 100,
    }
}
