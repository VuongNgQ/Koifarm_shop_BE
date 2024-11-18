using DataAccess.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Model.ResponseDTO
{
    public class ResponseConsignmentTypeDTO
    {
        public int ConsignmentTypeId { get; set; }
        public string? Name { get; set; }
    }

    public class FishConsignmentDTO
    {
        public int FishConsignmentId { get; set; }
        public int UserId { get; set; }
        public int? FishId { get; set; }
        public ConsignmentPurpose Purpose { get; set; }
        public ConsignmentType Type { get; set; }
        public decimal? Price { get; set; }
        public decimal? DepositAmount { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? TransferDate { get; set; }
        public DateTime? ReceiveDate { get; set; }
        public string? ConditionDescription { get; set; }
        public ConsignmentStatusEnum ConsignmentStatus { get; set; }
        public List<string>? ImageUrls { get; set; }
    }
}
