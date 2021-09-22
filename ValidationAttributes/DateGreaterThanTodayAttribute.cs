using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LecturesApp.ValidationAttributes
{
    public class DateGreaterThanTodayAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ErrorMessage = ErrorMessageString;
            var currentValue = (DateTime)value;

            var now = DateTime.Now;

            if (currentValue < now)
                return new ValidationResult(ErrorMessage);

            return ValidationResult.Success;
        }
    }
}
