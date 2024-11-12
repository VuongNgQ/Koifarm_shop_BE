using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Model.RequestDTO
{
    public class CreateOrderDTO
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public bool IsSent { get; set; }
        [Required]
        public string PaymentMethod { get; set; }
        public CreateAddressDTO CreateAddressDTO { get; set; }
    }
}
