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
        public string PaymentMethod { get; set; }
        public int UserId { get; set; }
        public string UserName {  get; set; }
        public string Phone { get; set; }
        public DateTime? OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime? CompleteDate { get; set; }
        public string Status { get; set; }
        public bool IsSent { get; set; }
        public ResponseAddressDTO Address { get; set; }
        public List<ResponseOrderItemDTO> Items { get; set; }
    }
}
