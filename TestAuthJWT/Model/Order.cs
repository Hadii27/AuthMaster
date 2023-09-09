using System.Text.Json.Serialization;
using TestAuthJWT.Model;

namespace AuthMaster.Model
{
    public class Order
    {
        public int Id { get; set; }

        public Guid UserId { get; set; }

        public decimal TotalPrice { get; set; }

        public DateTime DateTime { get; set; }

        [JsonIgnore]
        public List<OrderItems> orderItems { get; set; }

        public string address { get; set; }
    }
}
