using TodoAPI.Models.ValueObject;

namespace TodoAPI.Models.DTO;

public record class TodoDTO
{
    public int Id { get; init; }
    public string Timestamp { get; init; } = String.Empty;
    public TodoVO Todo { get; init; } = new TodoVO(); 
}
