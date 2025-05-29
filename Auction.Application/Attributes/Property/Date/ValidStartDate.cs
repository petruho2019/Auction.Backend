using System.ComponentModel.DataAnnotations;

namespace Auction.Application.Common.Attributes.Property.Date
{
    public class ValidStartDate : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value is DateTime dateTime)
            {
                if ((dateTime - (DateTime.Now - TimeSpan.FromMinutes(2)) ) < TimeSpan.Zero)
                {
                    ErrorMessage = "'Дата начала' не может быть раньше текущего времени";
                    return false;
                }

                return true;
            }

            return false;
        }
    }
}
