using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction.Application.Common.Attributes.Property.Date
{
    public class ValidEndDate : ValidationAttribute
    {
        public DateTime DateStart;

        public override bool IsValid(object? value)
        {
            if (value is DateTime dateEnd)
            {
                if ((DateStart - dateEnd) < TimeSpan.Zero)
                {
                    ErrorMessage = "'Дата окончания' не может быть раньше 'Даты начала'";
                    return false;
                }

                return true;
            }

            return false;
        }
    }
}
