using BusinessObject.Model.RequestDTO;
using DataAccess.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Model.ResponseDTO
{
    public class FishConsignmentDTO
    {
        public int FishConsignmentId { get; set; }
        public int UserId { get; set; }
        public int? FishId { get; set; }
        public string? Purpose { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? ConditionDescription { get; set; }
        public string? ConsignmentStatus { get; set; }
    }
    public class FishConsignmentCareResponseDTO
    {
        public int FishConsignmentId { get; set; }
        public int UserId { get; set; }
        public string? UserName {  get; set; }
        public int FishId { get; set; }
        public string? ConditionDescription { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? TransferDate { get; set; }
        public DateTime? ReceiveDate { get; set; }
        public string? Phone { get; set; }
        public string? Type { get; set; }
        public string? ConsignmentStatus { get; set; }
        public decimal? ServiceFee { get; set; }
        public string? ImageUrls { get; set; }
    }
    public class FishConsignmentSaleResponseDTO
    {
        public int FishConsignmentId { get; set; }
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public decimal? InitialPrice { get; set; }
        public decimal? FinalPrice { get; set; }
        public decimal? ServiceFee { get; set; }
        public decimal? CommissionFee { get; set; }
        public string? ConditionDescription { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Phone { get; set; }
        public FishInfoResponseDTO? FishInfo { get; set; }
        public string? Type { get; set; }
        public string? ConsignmentStatus { get; set; }
    }
    public class FishInfoResponseDTO
    {
        public string? Name { get; set; }
        public string? CategoryName { get; set; }
        public int? Age { get; set; }
        public string? Gender { get; set; }
        public string? Status { get; set; }
        public decimal? Size { get; set; }
        public string? ImageUrl { get; set; }
        public string? Video { get; set; }
    }
}
