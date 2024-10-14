using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entity
{
    public class OrderItem
    {
        public int OrderItemId { get; set; }
        public int? OrderId { get; set; }
        public int? FishId { get; set; }
        public int? PackageId { get; set; }
        public int? Quantity { get; set; }
        public decimal? Price { get; set; }
        public Order? Order { get; set; }
        public Fish? Fish { get; set; }
        public FishPackage? Package { get; set; }
    }
}
