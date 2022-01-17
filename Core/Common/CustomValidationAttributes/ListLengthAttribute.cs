using System.ComponentModel.DataAnnotations;

namespace Core.Common.CustomValidationAttributes
{
    public class ListLengthAttribute : ValidationAttribute
    {
        private readonly int _arrayLength;
        public ListLengthAttribute(int arrayLength)
        {
            _arrayLength = arrayLength;
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is IEnumerable<object>)
            {
                if(value is IEnumerable<object> items && items.Count() != _arrayLength)
                    return new ValidationResult($"Array length must be {_arrayLength}");
            }

            return ValidationResult.Success;
        }
    }
}