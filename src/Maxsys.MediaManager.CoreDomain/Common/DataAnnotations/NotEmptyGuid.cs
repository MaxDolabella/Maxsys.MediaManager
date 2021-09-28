namespace System.ComponentModel.DataAnnotations
{
    /// <summary>
    /// Specifies that a data field value is must not be an empty Guid.
    /// </summary>
    public class NotEmptyGuid : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var result = value is Guid guid && guid != Guid.Empty
                ? ValidationResult.Success
                : new ValidationResult($"{validationContext.MemberName} cannot be an empty guid.");

            return result;
        }
    }
}