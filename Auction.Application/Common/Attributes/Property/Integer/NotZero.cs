using System;
using System.ComponentModel.DataAnnotations;

namespace Auction.Application.Common.Attributes.Property.Integer
{
    [AttributeUsage(AttributeTargets.Property)]
    public class NotZero : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            string fieldName = validationContext.DisplayName;

            if (value is int intValue)
            {
                if (intValue <= 0)
                {
                    return new ValidationResult($"{fieldName} не может быть {(intValue == 0 ? "равен 0" : "отрицательным")}");
                }

                return ValidationResult.Success;
            }

            if (value is long longValue)
            {
                if (longValue <= 0)
                {
                    return new ValidationResult($"{fieldName} не может быть {(longValue == 0 ? "равен 0" : "отрицательным")}");
                }

                return ValidationResult.Success;
            }

            return new ValidationResult($"{fieldName}: некорректный тип данных");
        }
    }
}
