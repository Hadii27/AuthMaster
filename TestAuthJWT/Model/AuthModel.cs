using Microsoft.EntityFrameworkCore;

namespace TestAuthJWT.Model
{
    public class AuthModel 
    {
        public string Message { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public DateTime ExpireOn { get; set; }
        public bool isAuthenticated { get; set; }
        public List<string> Roles { get; set; }
    }
}
