using System.ComponentModel.DataAnnotations;
using TodoAPI.Models.Constants;
using TodoAPI.Models.Validations;
using TodoAPI.Models.ValueObject;

namespace TodoAPI.Models.DTO;

public record class TodoQueryDTO : IPage
{
    [MaxLength(TodoRestriction.MAX_AUTHOR)]
    [MinLength(TodoRestriction.MIN_AUTHOR)]
    public string? Author { get; init; }

    [MaxLength(TodoRestriction.MAX_SEARCH)]
    [MinLength(TodoRestriction.MIN_SEARCH)]
    public string? SearchQuery { get; init; }

    public bool? Done { get; init; }

    [PageSizeValidation]
    public int PageSize { get; init; } = 10;

    [Range(1, int.MaxValue)]
    public int PageNr { get; init; } = 1;
}
