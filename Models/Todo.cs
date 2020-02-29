using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ex1_ToDo.Models
{
    public class Todo
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Header { get; set; }

        [Required]
        [Range(0, 5)]
        public int Priority { get; set; }

        [Required]
        public DateTime CreatedTime { get; set; }

        public ICollection<Item> Items { get; set; }

        [EmailAddress]
        [ForeignKey("Author")]
        public string AuthorEmail { get; set; }

        [Required]
        public User Author { get; set; }
    }
}