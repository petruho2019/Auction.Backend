using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction.Application.Common.Attributes.Property.Date
{
    public class ValidStartDate : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value is DateTime dateTime)
            {
                if ((dateTime - (DateTime.Now - TimeSpan.FromMinutes(5)) ) < TimeSpan.Zero)
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
