using System.ComponentModel.DataAnnotations;

namespace TestAuthJWT.Model
{
    public class TokenRequestModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
