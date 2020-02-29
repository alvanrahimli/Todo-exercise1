using System.ComponentModel.DataAnnotations;

namespace ToDo_exercise1.Models.Dtos
{
    public class ItemCreateDto
    {
        [Required]
        [StringLength(120, MinimumLength = 3)]
        public string Content { get; set; }

        [Required]
        public bool IsDone { get; set; }

        [Required]
        public int Order { get; set; }

        [Required]
        public int TodoId { get; set; }
    }
}