using DataAccess.Enum;

namespace DataAccess.Entity
{
    public class Fish
    {
        public int FishId { get; set; }
        public string? Name { get; set; }
        public int? Age { get; set; }
        public FishGenderEnum Gender { get; set; }
        public decimal? Size { get; set; }
        public string? Description { get; set; }
        public int? CategoryId { get; set; }
        public decimal? Price { get; set; }
        public decimal? DailyFood { get; set; }
        public string? ImageUrl { get; set; }
        public int? QuantityInStock { get; set; }
        public FishStatusEnum Status { get; set; }
        public Category? Category { get; set; }
        
        public ICollection<OrderItem>? OrderItems { get; set; }
        public ICollection<CartItem>? CartItems { get; set; }
        public ICollection<Feedback>? Feedbacks { get; set; }
        public ICollection<FishConsignment>? Consignments { get; set; }
        public ICollection<SubImage>? SubImages { get; set; }
    }

    public enum FishGenderEnum
    {
        Male,
        Female,
        Unknown
    }
}
