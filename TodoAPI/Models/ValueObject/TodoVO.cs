using System.ComponentModel.DataAnnotations;
using TodoAPI.Models.Constants;

namespace TodoAPI.Models.ValueObject;

public record class TodoVO()
{
    [Required]
    [MaxLength(TodoRestriction.MAX_TITLE)]
    [MinLength(TodoRestriction.MIN_TITLE)]
    public required string Title { get; init; }
    
    [Required]
    [MaxLength(TodoRestriction.MAX_AUTHOR)]
    [MinLength(TodoRestriction.MIN_AUTHOR)]
    public required string Author { get; init; }

    [Required]
    [MaxLength(TodoRestriction.MAX_DESCRIPTION)]
    [MinLength(TodoRestriction.MIN_DESCRIPTION)]
    public required string Description { get; init; }

    [Required]
    public required bool Done { get; init; }
}
