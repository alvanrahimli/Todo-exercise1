using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ex1_ToDo.Models
{
    public class User
    {
        //[Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Username { get; set; }
        public string NormalizedUsername { get; set; }

        [Key]
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string NormalizedEmail { get; set; }

        [Required]
        public byte[] PasswordHash { get; set; }
        public ICollection<Todo> Todos { get; set; }
        public int TodoCount
        {
            get
            {
                return Todos == null ? 0 : Todos.Count;
            }
        }
    }
}