using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System;

namespace eTraveller.ValidationTest
{
    public class ValidationOneMonthAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            value = (DateTime)value;
            // This assumes inclusivity, i.e. exactly one month after is okay
            if (DateTime.Now.AddMonths(+1).CompareTo(value) >= 0 && DateTime.Now.CompareTo(value) <= 0)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("Date must be in future years!");
            }
        }
    }
}
