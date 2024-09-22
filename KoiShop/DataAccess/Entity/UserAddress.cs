using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entity
{
    public class UserAddress
    {
        public int? UserId { get; set; }
        public User? User { get; set; }

        public int? AddressId { get; set; }
        public Address? Address { get; set; }
    }
}
