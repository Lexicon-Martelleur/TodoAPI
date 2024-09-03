namespace TodoAPI.Models.ValueObject;

public interface IPage
{
    int PageSize { get; init; }

    int PageNr { get; init; }
}
