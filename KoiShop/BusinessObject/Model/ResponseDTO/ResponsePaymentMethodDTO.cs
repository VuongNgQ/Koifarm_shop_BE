using DataAccess.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Model.ResponseDTO
{
    public class ResponsePaymentMethodDTO
    {
        public int PaymentMethodId { get; set; }
        public string Name { get; set; }
    }

    public class PaymentDTO
    {
        public int? TransactionId { get; set; }

        public TransactionPurpose TransactionType { get; set; }
        public int? RelatedId { get; set; }
        public decimal Amount { get; set; }
        public PaymentMethod Method { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime PaymentDate { get; set; }
        public PaymentStatus Status { get; set; }
        public string? Description { get; set; }
    }
}
