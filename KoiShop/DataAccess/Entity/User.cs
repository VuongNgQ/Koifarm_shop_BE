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
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string PasswordHash { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public required string Phone { get; set; }
        public int? RoleId { get; set; }
        public UserStatusEnum Status { get; set; }
        public Role? Role { get; set; }
        public ICollection<UserAddress>? UserAddresses { get; set; }
        public UserCart UserCart { get; set; }
        public ICollection<Order>? Orders { get; set; }
        public ICollection<Feedback>? Feedbacks { get; set; }
        public ICollection<FishConsignment>? FishConsignments { get; set; }
        public ICollection<PasswordResetToken>? PasswordResetTokens { get; set; }
        public ICollection<UserFishOwnership>? UserFishOwnerships { get; set; }
    }

    public enum UserStatusEnum
    {
        Active,
        Disable
    }
}
