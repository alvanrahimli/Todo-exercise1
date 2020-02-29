using System.ComponentModel.DataAnnotations;

namespace ToDo_exercise1.Models.Dtos
{
    public class UserLogin
    {
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Parameter { get; set; }
        public bool IsEmail
        {
            get
            {
                if (this.Parameter.Contains('@'))
                    return true;
                return false;
            }
        }

        [Required]
        [StringLength(50, MinimumLength = 8)]
        public string Password { get; set; }
    }
}