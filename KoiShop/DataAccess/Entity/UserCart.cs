using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entity
{
    public class UserCart
    {
        public int UserCartId { get; set; }
        public int? UserId { get; set; }

        public User? User { get; set; }
        public ICollection<CartItem>? CartItems { get; set; }
    }
}
