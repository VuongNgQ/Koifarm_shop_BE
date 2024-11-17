using DataAccess.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Model.RequestDTO
{
    public class CreateConsignmentTypeDTO
    {
        public string Name { get; set; }
    }
    public class CreateConsignmentDTO
    {
        public int UserId { get; set; }
        public int? FishId { get; set; }
        public ConsignmentPurpose Purpose { get; set; }
        public bool IsFromShop { get; set; }
        public decimal? Price { get; set; }
        public decimal? DepositAmount { get; set; }
        public DateTime? TransferDate { get; set; }
        public DateTime? ReceiveDate { get; set; }
        public string? ConditionDescription { get; set; }
        public List<string>? ImageUrls { get; set; }
        public string? VideoUrl { get; set; }
    }
    public class ApproveConsignmentDTO
    {
        public int ConsignmentId { get; set; }
        public decimal? AgreedPrice { get; set; }
        public decimal? DepositAmount { get; set; }
    }
}
