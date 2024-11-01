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
        public int? FishId { get; set; }
        public int? ConsignmentTypeId { get; set; }
        public decimal? Price { get; set; }
        public DateTime? CreateDate { get; set; } = DateTime.Now;
        public DateTime? TransferDate { get; set; }
        public DateTime? ReceiveDate { get; set; }
        public ConsignmentStatusEnum ConsignmentStatusId { get; set; }
        public int? FishStatusId { get; set; }
        public string? ConditionDescription { get; set; }
        public List<string>? ImageUrls { get; set; } = new List<string>();
        public string? VideoUrl { get; set; }
        public User? User { get; set; }
        public Fish? Fish { get; set; }
        public ConsignmentType? ConsignmentType { get; set; }
        
    }
}
