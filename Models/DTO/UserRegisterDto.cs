using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SecondTimeAttempt.Models.DTO
{
    public class UserRegisterDto
    {
        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(5, ErrorMessage = "You cannot set a password less than 5 characters.")]
        public string Password { get; set; }

    }
}



