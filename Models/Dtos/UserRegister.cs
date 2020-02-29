using System.ComponentModel.DataAnnotations;

namespace ToDo_exercise1.Models.Dtos
{
    public class UserRegister
    {
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Username { get; set; }

        [EmailAddress]
        [Required]
        public string Email { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 8)]
        public string Password { get; set; }
    }
}