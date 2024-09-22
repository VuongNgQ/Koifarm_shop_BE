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
        public DateTime? CreateDate { get; set; }
        public DateTime? TransferDate { get; set; }
        public DateTime? ReceiveDate { get; set; }
        public int? ConsignmentStatusId { get; set; }
        public int? FishStatusId { get; set; }
        public User? User { get; set; }
        public FishSingle? Fish { get; set; }
        public ConsignmentType? ConsignmentType { get; set; }
        public ConsignmentStatus? ConsignmentStatus { get; set; }
    }
}
