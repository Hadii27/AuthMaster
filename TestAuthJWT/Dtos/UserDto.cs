using System.ComponentModel.DataAnnotations;

namespace AuthMaster.Dtos
{
    public class UserDto
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }
    }
}
