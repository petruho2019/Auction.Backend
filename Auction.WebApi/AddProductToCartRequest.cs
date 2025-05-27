using Auction.Application.Common.Attributes.Property.Integer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WakeupStore.Contracts.Models
{
    class AddProductToCartRequest
    {
        
        [Required(ErrorMessage = "Size cannot by empry")]
        [NotZero]
        public int Size { get; set; }

        [Required(ErrorMessage = "Quantity cannot by empry")]
        [NotZero]
        public int Quantity { get; set; }

        [MaxLength(150, ErrorMessage = "Desription cannot by more than 150 characters")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Name cannot by empry")]
        [MaxLength(20, ErrorMessage = "Name cannot by more than 20")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Price cannot by empry")]
        [NotZero]
        public decimal Price { get; set; }

    }
}
