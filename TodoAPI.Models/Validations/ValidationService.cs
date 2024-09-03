using System.ComponentModel.DataAnnotations;

namespace TodoAPI.Models.Validations;

/// <summary>
/// Utility class used for validation.
/// </summary>
public static class ValidationService
{
    /// <summary>
    /// A utility method used to validate an object.
    /// </summary>
    /// <typeparam name="PropertyType"></typeparam>
    /// <param name="instance"></param>
    /// <exception cref="ArgumentException"></exception>
    public static InstanceType ValidateInstance<InstanceType>(
        InstanceType instance)
    {
        if (instance == null)
        {
            throw new ArgumentNullException("Can not validate an instance that is null");
        }

        var context = new ValidationContext(instance);
        var validations = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(
            instance, context, validations, true
        );

        if (isValid) { return instance; }

        foreach (var validation in validations)
        {
            if (validation == ValidationResult.Success)
            {
                continue;
            }
            throw new ArgumentException(
                validation.ErrorMessage ?? $"Invalid state of {nameof(instance)}"
            );
        }

        return instance;
    }
}
