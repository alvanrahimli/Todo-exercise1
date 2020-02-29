using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ex1_ToDo.Models
{
    public class Item
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(120, MinimumLength = 3)]
        public string Content { get; set; }

        [Required]
        public bool isDone { get; set; }

        [Required]
        public int Order { get; set; }

        [ForeignKey("Todo")]
        public int TodoId { get; set; }

        [Required]
        public Todo Todo { get; set; }
    }
}