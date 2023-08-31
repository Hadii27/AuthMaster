using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AuthMaster.Model
{
    public class ProductModel
    {
        [Key]
        public int ProductId { get; set; }
        [Required]
        public string ProductName { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        public string ProductDescription { get; set; }

        [Required]
        public int Quantity { get; set; }

        public byte[] image { get; set; }
        [Required]
        public int categoryId { get; set; }

        public  CategoryModel category { get; set; }


    }
}
