using System.ComponentModel.DataAnnotations;

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

        return IsUnixTimeStamp(input);
    }

    private bool IsUnixTimeStamp(string input)
    {
        if (!long.TryParse(input, out long timestamp))
        {
            return false;
        }
        long maxUnixTimestamp = new DateTimeOffset(2038, 1, 19, 3, 14, 0, TimeSpan.Zero).ToUnixTimeSeconds();
        // long maxUnixTimestamp = DateTimeOffset.MaxValue.ToUnixTimeSeconds();

        long minUnixTimestamp = 0;
        return timestamp >= minUnixTimestamp &&
            timestamp <= maxUnixTimestamp;
    }
}
