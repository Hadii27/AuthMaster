using System.ComponentModel.DataAnnotations;

namespace AuthMaster.Dtos
{
    public class ItemsDto
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        public int Quantity { get; set; }
    }
}
