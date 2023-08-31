using System.ComponentModel.DataAnnotations;

namespace AuthMaster.Model
{
    public class CartItems
    {
        public int id { get; set; }
        public int ProductId { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public double price { get; set; }

        public int cartID { get; set; }

        public Cart cart { get; set; }
    }
}
