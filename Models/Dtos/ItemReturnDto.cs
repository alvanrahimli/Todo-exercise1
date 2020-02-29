namespace ToDo_exercise1.Models.Dtos
{
    public class ItemReturnDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public bool isDone { get; set; }
        public int Order { get; set; }
        public int TodoId { get; set; }
    }
}