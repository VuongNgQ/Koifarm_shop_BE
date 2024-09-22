using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entity
{
    public class PaymentMethod
    {
        public int PaymentMethodId { get; set; }
        public string? Name { get; set; }

        public ICollection<Order>? Orders { get; set; }
    }
}
