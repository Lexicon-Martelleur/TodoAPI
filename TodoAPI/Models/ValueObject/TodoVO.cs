using System.ComponentModel.DataAnnotations;
using TodoAPI.Models.Constants;

namespace TodoAPI.Models.ValueObject;

public record class TodoVO()
{
    [Required]
    [MaxLength(TodoRestriction.MAX_TITLE)]
    public string Title { get; init; } = string.Empty;

    
    [Required]
    [MaxLength(TodoRestriction.MAX_AUTHOR)]
    public string Author { get; init; } = string.Empty;

    [Required]
    [MaxLength(TodoRestriction.MAX_DESCRIPTION)]
    public string Description { get; init; } = string.Empty;

    [Required]
    public bool Done { get; init; }
}
