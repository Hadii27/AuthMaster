using System.ComponentModel.DataAnnotations;

namespace AuthMaster.Dtos
{
    public class ProductDto
    {
        public string ProductName { get; set; }
        [Required]

        public double Price { get; set; }
        [Required]

        public string ProductDescription { get; set; }
        [Required]
        public int Quantity { get; set; }

        public IFormFile image { get; set; }

        [Required]
        public int categoryId { get; set; }
    }
}
