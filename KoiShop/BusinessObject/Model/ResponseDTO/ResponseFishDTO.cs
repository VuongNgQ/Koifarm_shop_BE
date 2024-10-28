using DataAccess.Entity;
using DataAccess.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Model.ResponseDTO
{
    public class ResponseFishDTO
    {
        public int FishId { get; set; }
        public string? Name { get; set; }
        public int? Age { get; set; }
        public FishGenderEnum Gender { get; set; }
        public decimal? Size { get; set; }
        public string? Description { get; set; }
        public string? CategoryName { get; set; }
        public decimal? Price { get; set; }
        public decimal? DailyFood { get; set; }
        public string? ImageUrl { get; set; }
        public int? QuantityInStock { get; set; }
        public ProductStatusEnum Status { get; set; }
    }
}
