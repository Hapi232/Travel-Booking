using System.ComponentModel.DataAnnotations;

namespace Travel_Booking.Models
{
    public class EndAfterStartAttribute : ValidationAttribute
    {
        private readonly string _startPropertyName;

        public EndAfterStartAttribute(string startPropertyName)
        {
            _startPropertyName = startPropertyName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var startProperty = validationContext.ObjectType.GetProperty(_startPropertyName);
            if (startProperty == null)
                return new ValidationResult($"Unknown property: {_startPropertyName}");

            var startValue = (DateTime)startProperty.GetValue(validationContext.ObjectInstance);
            var endValue = (DateTime)value;

            if (endValue < startValue)
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
