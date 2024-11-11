using DataAccess.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entity
{
    public class Payment
    {
        public int PaymentId { get; set; }
        public int? UserId { get; set; }
        public User? User { get; set; }
        public TransactionType TransactionType { get; set; }
        public int? RelatedId { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public decimal Amount { get; set; }
        public string? Currency { get; set; }
        public string? TransactionId { get; set; }
        public string? PaymentUrl { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        public DateTime PaymentDate { get; set; }
        public DateTime? ExpireDate { get; set; }
        public string? Description { get; set; }
    }

}
