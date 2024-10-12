using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Model.ResponseDTO
{
    public class ResponseOrderDTO
    {
        public int OrderId { get; set; }
        public DateTime? OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalPrice { get; set; }
        public int StatusId { get; set; }
        public int PaymentMethodId { get; set; }
        public bool IsSent { get; set; }
        public ResponseAddressDTO Address { get; set; }
        public ResponseUserDTO User { get; set; }
    }
}
