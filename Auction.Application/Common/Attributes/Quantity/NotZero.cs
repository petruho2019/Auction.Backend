using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction.Application.Common.Attributes.Quantity
{
    class NotZero : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value is int quantity)
            {
                if (quantity == 0)
                {
                    ErrorMessage = "Количество не может быть равно 0";
                    return false;
                }
                if (quantity < 0 )
                {
                    ErrorMessage = "Количество не может быть отрицательным";
                    return false;
                }

                return true;
            }

            ErrorMessage = "Некорректный тип данных";
            return false;
        }
    }
}
