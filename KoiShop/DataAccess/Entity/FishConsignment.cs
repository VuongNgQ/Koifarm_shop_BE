using DataAccess.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entity
{
    public class FishConsignment
    {
        public int FishConsignmentId { get; set; }
        public int? UserId { get; set; }
        public int FishId { get; set; }
        public ConsignmentPurpose Purpose { get; set; } //Care, Sale
        public ConsignmentType Type { get; set; }
        public decimal? InitialPrice { get; set; }
        public decimal? FinalPrice { get; set; }
        public decimal? ServiceFee { get; set; }
        public decimal? CommissionFee { get; set; }
        public DateTime? CreateDate { get; set; } = DateTime.Now;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int ConsignmentDurationMonths { get; set; }
        public ConsignmentStatusEnum ConsignmentStatus { get; set; }
        public string? Phone { get; set; }
        public string? ConditionDescription { get; set; }
        public string? ImageUrls { get; set; }
        public string? Video {  get; set; }
        public User? User { get; set; }
        public Fish? Fish { get; set; }
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}
