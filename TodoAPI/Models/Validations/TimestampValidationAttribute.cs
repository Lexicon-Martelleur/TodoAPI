using System.ComponentModel.DataAnnotations;
using TodoAPI.Models.Constants;

namespace TodoAPI.Models.Validations;

public class TimestampValidationAttribute : ValidationAttribute
{
    public override bool IsValid(
        object? value)
    {
        if (value is not string input)
        {
            return false;
        }

        var errorMessage = $"Pagination page size must be in interval [" +
            $"{PaginationRestriction.MIN_PAGE_SIZE}," +
            $"{PaginationRestriction.MAX_PAGE_SIZE}]";

        return IsUnixTimeStamp(input);
    }

    private bool IsUnixTimeStamp(string input)
    {
        if (!long.TryParse(input, out long timestamp))
        {
            return false;
        }
        long minUnixTimestamp = 0;
        long maxUnixTimestamp = DateTimeOffset.MaxValue.ToUnixTimeSeconds();
        return timestamp >= minUnixTimestamp &&
            timestamp <= maxUnixTimestamp;
    }
}
