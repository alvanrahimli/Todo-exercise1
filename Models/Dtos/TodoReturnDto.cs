using System;
using System.Collections.Generic;
using ex1_ToDo.Models;

namespace ToDo_exercise1.Models.Dtos
{
    public class TodoReturnDto
    {
        public int Id { get; set; }
        public string Header { get; set; }
        public int Priority { get; set; }
        public DateTime CreatedTime { get; set; }
        public ICollection<ItemReturnDto> Items { get; set; }
    }
}