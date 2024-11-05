using DataAccess.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Model.RequestDTO
{
    public class CreatePaymentMethodDTO
    {
        public string Name { get; set; }
    }
    public class CreatePaymentDTO
    {
        public int? UserId { get; set; }
        public TransactionType TransactionType { get; set; }
        public int? RelatedId { get; set; }
        public decimal Amount { get; set; }
        public PaymentStatus Status { get; set; }
        public string? Description { get; set; }
    }

    public class UpdatePaymentDTO
    {
        public PaymentStatus Status { get; set; }
        public string? Description { get; set; }
    }
}
