using DataAccess.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entity
{
    public class FishPackage
    {
        public int FishPackageId { get; set; }
        public string? Name { get; set; }
        public int? Age { get; set; }
        public string? Gender { get; set; }
        public decimal? Size { get; set; }
        public string? Description { get; set; }
        
        public decimal? TotalPrice { get; set; }
        public decimal? DailyFood { get; set; }
        public string? ImageUrl { get; set; }
        public int? NumberOfFish { get; set; }
        
        public ProductStatusEnum Status { get; set; }
        
        
        public ICollection<OrderItem>? OrderItems { get; set; }
        public ICollection<CartItem>? CartItems { get; set; }
        public ICollection<PackageConsignment>? Consignments { get; set; }
        public ICollection<Feedback>? Feedbacks { get; set; }
    }
}
