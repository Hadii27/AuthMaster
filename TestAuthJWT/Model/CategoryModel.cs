using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AuthMaster.Model
{
    public class CategoryModel
    {
        [Key]
        public int CategoryId { get; set; }
        [Required]
        public string CategoryName { get; set; }
        [JsonIgnore]
        public ICollection<ProductModel> Products { get; set; }

    }
}
