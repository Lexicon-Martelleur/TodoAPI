using System.ComponentModel.DataAnnotations;
using TodoAPI.Models.Constants;

namespace TodoAPI.Models.DTO;

public record class TodoQueryDTO(
    [MaxLength(TodoRestriction.MAX_AUTHOR)] string? Author,
    [MaxLength(TodoRestriction.MAX_AUTHOR)] string? SearchQuery,
    bool? Done,
    [Range(1, 100)] int PageSize = 10,
    [Range(1, int.MaxValue)] int PageNr = 1
);
