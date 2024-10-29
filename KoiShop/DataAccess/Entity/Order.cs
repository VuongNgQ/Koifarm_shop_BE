using DataAccess.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entity
{
    public class Order
    {
        public int OrderId { get; set; }
        public int? UserId { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? CompleteDate { get; set; }
        public int? AddressId { get; set; }
        public decimal? TotalPrice { get; set; }
        public OrderStatusEnum Status { get; set; }
        public int? PaymentMethodId { get; set; }
        public bool? IsSent { get; set; }
        public User? User { get; set; }
        public Address? Address { get; set; }
        public PaymentMethod? PaymentMethod { get; set; }
        public ICollection<OrderItem>? OrderItems { get; set; }
    }
}
