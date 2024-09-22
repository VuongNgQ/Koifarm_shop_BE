using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entity
{
    public class Address
    {
        public int AddressId { get; set; }
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? District { get; set; }
        public int? UserId { get; set; }
        public ICollection<Order>? Orders { get; set; }
        public ICollection<UserAddress>? UserAddresses { get; set; }
    }
}
