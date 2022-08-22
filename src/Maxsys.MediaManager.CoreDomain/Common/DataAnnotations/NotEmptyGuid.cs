namespace System.ComponentModel.DataAnnotations
{
    /// <summary>
    /// Specifies that a data field value is must not be an empty Guid.
    /// </summary>
    public class NotEmptyGuid : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            return value is not null
                && value is Guid guid
                && guid != Guid.Empty
                ? ValidationResult.Success
                : new ValidationResult(ErrorMessage ?? $"{validationContext.MemberName} cannot be an empty guid.");
        }
    }
}