using System.ComponentModel.DataAnnotations;
using TodoAPI.Models.Validations;
using TodoAPI.Models.ValueObject;

namespace TodoAPI.Models.DTO;

public record class TodoDTO
{
    public required int Id { get; init; }

    [Required]
    [TimestampValidation]
    public required string Timestamp { get; init; }

    [Required]
    public required TodoVO Todo { get; init; } 
}
