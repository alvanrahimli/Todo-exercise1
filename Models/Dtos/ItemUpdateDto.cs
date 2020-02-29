using System.ComponentModel.DataAnnotations;

namespace ToDo_exercise1.Models.Dtos
{
    public class ItemUpdateDto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Content { get; set; }
        public bool IsDone { get; set; }
        public int Order { get; set; }
    }
}