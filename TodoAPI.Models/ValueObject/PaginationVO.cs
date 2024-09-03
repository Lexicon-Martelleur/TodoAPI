using TodoAPI.Models.Validations;

namespace TodoAPI.Models.ValueObject;

public class PaginationVO : IPage
{
    public required int TotalItemCount { get; init; }

    [PageSizeValidation]
    public required int PageSize { get; init; }

    public required int PageNr { get; init; }

    public int TotalPageCount => (int)Math.Ceiling(TotalItemCount / (double)PageSize);

}
