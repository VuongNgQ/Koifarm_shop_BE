using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entity
{
    public class UserFishOwnership
    {
        public int OwnershipId { get; set; }
        public int UserId { get; set; }
        public int FishId { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string? Notes { get; set; }
        public User? User { get; set; }
        public Fish? Fish { get; set; }
    }
}
