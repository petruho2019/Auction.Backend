using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction.Application.Common.Attributes.Field.Date
{
    class ValidStartDate : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value is DateTime dateTime)
            {
                if ((DateTime.Now - dateTime) < TimeSpan.Zero)
                {
                    ErrorMessage = "'Время начала' не может быть раньше текущего времени";
                    return false;
                }

                return true;
            }

            return false;
        }
    }
}
