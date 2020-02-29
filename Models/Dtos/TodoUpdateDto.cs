using System.ComponentModel.DataAnnotations;

namespace ToDo_exercise1.Models.Dtos
{
    public class TodoUpdateDto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Header { get; set; }

        [Required]
        [Range(0, 5)]
        public int Priority { get; set; }
    }
}