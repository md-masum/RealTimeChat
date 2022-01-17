using System.ComponentModel.DataAnnotations;

namespace Core.Common.CustomValidationAttributes
{
    public class ListMinLengthAttribute: ValidationAttribute
    {
        private readonly int _minLength;

        public ListMinLengthAttribute(int minLength)
        {
            _minLength = minLength;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is IEnumerable<object>)
            {
                if (value is IEnumerable<object> items && items.Count() < _minLength)
                    return new ValidationResult($"Array minimum length must be {_minLength}");
            }

            return ValidationResult.Success;
        }
    }
}