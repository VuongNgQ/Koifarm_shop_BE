using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entity
{
    public class OrderStatus
    {
        public int OrderStatusId { get; set; }
        public string? Name { get; set; }

        public ICollection<Order>? Orders { get; set; }
    }
}
