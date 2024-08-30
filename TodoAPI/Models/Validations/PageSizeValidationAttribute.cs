using System.ComponentModel.DataAnnotations;
using TodoAPI.Models.Constants;
using TodoAPI.Models.ValueObject;

namespace TodoAPI.Models.Validations;

public class PageSizeValidationAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(
        object? value,
        ValidationContext validationContext)
    {
        if (value is not int input ||
            validationContext.ObjectInstance is not IPage page)
        {
            return new ValidationResult($"Validation {nameof(PageSizeValidationAttribute)} error");
        }

        var errorMessage = $"Pagination page size must be in interval [" +
            $"{PaginationRestriction.MIN_PAGE_SIZE}," +
            $"{PaginationRestriction.MAX_PAGE_SIZE}]";

        return (page.PageSize < PaginationRestriction.MIN_PAGE_SIZE ||
            page.PageSize > PaginationRestriction.MAX_PAGE_SIZE)
            ? new ValidationResult(errorMessage)
            : ValidationResult.Success;
    }
}
