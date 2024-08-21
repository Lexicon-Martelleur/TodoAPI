namespace TodoAPI.Models.ValueObject;

public record class TodoVO()
{
    public string Title { get; init; } = string.Empty;
    public string Author { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public bool Done { get; init; }
}
