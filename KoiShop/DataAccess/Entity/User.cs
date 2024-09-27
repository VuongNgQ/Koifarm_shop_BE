using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entity
{
    public class User
    {
        public int UserId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Phone { get; set; }
        public int? RoleId { get; set; }
        public string? Status { get; set; }
        public Role? Role { get; set; }
        public ICollection<UserAddress>? UserAddresses { get; set; }
        public ICollection<UserCart>? UserCarts { get; set; }
        public ICollection<Order>? Orders { get; set; }
        public ICollection<Feedback>? Feedbacks { get; set; }
        public ICollection<FishConsignment>? FishConsignments { get; set; }
        public ICollection<PackageConsignment>? PackageConsignments { get; set; }
    }
}
