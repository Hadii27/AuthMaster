namespace AuthMaster.Model
{
    public class OrderItems
    {
        public int id { get; set; }

        public decimal PriceAtOrderTime { get; set; }

        public int OrderId { get; set; }

        public Order order { get; set; }

        public int productId { get; set; }

        public ProductModel product { get; set; }

        public int Quantity { get; set; }
    }
}
