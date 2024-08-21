using System.ComponentModel.DataAnnotations;
using TodoAPI.Models.Constants;
using TodoAPI.Models.ValueObject;

namespace TodoAPI.Models.DTO;

public record class TodoDTO
{
    public int Id { get; init; }

    [Required]
    [MaxLength(TodoRestriction.MAX_TIMESTAMP)]
    public string Timestamp { get; init; } = String.Empty;

    [Required]
    public TodoVO Todo { get; init; } = new TodoVO(); 
}
