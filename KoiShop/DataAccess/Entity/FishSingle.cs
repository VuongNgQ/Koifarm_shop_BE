using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entity
{
    public class FishSingle
    {
        public int FishSingleId { get; set; }
        public string? Name { get; set; }
        public int? Age { get; set; }
        public string? Gender { get; set; }
        public decimal? Size { get; set; }
        public string? Description { get; set; }
        public int? CategoryId { get; set; }
        public decimal? Price { get; set; }
        public decimal? DailyFood { get; set; }
        public string? ImageUrl { get; set; }
        public int? QuantityInStock { get; set; }
        public int? StatusId { get; set; }
        public Category? Category { get; set; }
        public ProductStatus? Status { get; set; }
        public ICollection<OrderItem>? OrderItems { get; set; }
        public ICollection<CartItem>? CartItems { get; set; }
        public ICollection<Feedback>? Feedbacks { get; set; }
        public ICollection<FishConsignment>? Consignments { get; set; }
    }
}
