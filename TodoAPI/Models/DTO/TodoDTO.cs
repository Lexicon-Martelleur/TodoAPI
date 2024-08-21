using TodoAPI.Models.ValueObject;

namespace TodoAPI.Models.DTO
{
    public class TodoDTO
    {
        public int Id { get; set; }
        public string TimeStamp { get; set; }
        public TodoVO Todo { get; set; } = new TodoVO(); 

    }
}
