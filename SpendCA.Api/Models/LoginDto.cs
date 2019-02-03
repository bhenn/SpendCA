using System.ComponentModel.DataAnnotations;

namespace SpendCA.Models
{
    public class LoginDto
    {
        
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

    }

}