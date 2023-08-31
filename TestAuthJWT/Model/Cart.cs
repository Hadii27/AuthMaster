using AuthMaster.Model;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace AuthMaster.Model
{
    public class Cart
    {
        public int Id { get; set; }

        public Guid userID { get; set; }

        [JsonIgnore]
        public List<CartItems> CartItems { get; set; } // Use List<CartItem> instead of DbSet<CartItems>

    }
}
