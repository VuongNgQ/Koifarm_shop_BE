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
        public int UserId { get; set; }
        public string UserName {  get; set; }
        public DateTime? OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; }
        public int PaymentMethodId { get; set; }
        public bool IsSent { get; set; }
        public ResponseAddressDTO Address { get; set; }
        public List<ResponseOrderItemDTO> Items { get; set; }
    }
}
