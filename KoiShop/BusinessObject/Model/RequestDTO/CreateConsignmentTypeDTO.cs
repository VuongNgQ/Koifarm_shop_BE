using DataAccess.Enum;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Model.RequestDTO
{
    public class CreateConsignmentDTO
    {
        public int UserId { get; set; }
        public ConsignmentType Type { get; set; }
        public string? ConditionDescription { get; set; }
        public decimal? Price { get; set; }
    }
    public class CareConsignmentDTO : CreateConsignmentDTO
    {
        public int FishId { get; set; }
        public DateTime? TransferDate { get; set; }
        public DateTime? ReceiveDate { get; set; }
    }
    public class SaleConsignmentDTO : CreateConsignmentDTO
    {
        public FishInfoDTO? FishInfo { get; set; }
    }
    public class FishInfoDTO
    {
        public string? Name { get; set; }
        public int CategoryId { get; set; }
        public int? Age { get; set; }
        public FishGenderEnum Gender { get; set; }
        public FishStatusEnum Status { get; set; }
        public decimal? Size { get; set; }
        public IFormFile? ImageUrl { get; set; }
    }
    public class ApproveConsignmentDTO
    {
        public decimal AgreedPrice { get; set; }
        public decimal FishSaleAmount { get; set; }
    }
}
