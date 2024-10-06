using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entity
{
    public class PackageConsignment
    {
        public int PackageConsignmentId { get; set; }
        public int? UserId { get; set; }
        public int? PackageId { get; set; }
        public int? ConsignmentTypeId { get; set; }
        public decimal? Price { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? TransferDate { get; set; }
        public DateTime? ReceiveDate { get; set; }
        public int? ConsignmentStatusId { get; set; }
        public int? PackageStatusId { get; set; }
        public User? User { get; set; }
        public FishPackage? Package { get; set; }
        public ConsignmentType? ConsignmentType { get; set; }
        public ConsignmentStatus? ConsignmentStatus { get; set; }
        public FishStatus? FishStatus { get; set; }
    }
}
