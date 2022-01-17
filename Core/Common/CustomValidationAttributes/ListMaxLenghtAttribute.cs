using System.ComponentModel.DataAnnotations;

namespace Core.Common.CustomValidationAttributes
{
    public class ListMaxLengthAttribute: ValidationAttribute
    {
        private readonly int _maxLength;

        public ListMaxLengthAttribute(int maxLength)
        {
            _maxLength = maxLength;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is IEnumerable<object>)
            {
                if (value is IEnumerable<object> items && items.Count() > _maxLength)
                    return new ValidationResult($"Array maximum length must be {_maxLength}");
            }

            return ValidationResult.Success;
        }
    }
}