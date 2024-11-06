using BusinessObject.Model.ResponseDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Model.RequestDTO.UpdateReq.Entity
{
    public class UpdateOrderDTO
    {
        public string? Status { get; set; }
        public int? PaymentMethodId { get; set; }
        public bool? IsSent { get; set; }
        public UpdateAddressDTO? Address { get; set; }
    }

    public class UpdateAddressDTO
    {
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? District { get; set; }
    }

}
