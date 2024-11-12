using DataAccess.Entity;
using DataAccess.Enum;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Model.RequestDTO
{
    public class CreateFishDTO
    {
        public string? Name { get; set; }
        public int? Age { get; set; }
        public FishGenderEnum Gender { get; set; }
        public decimal? Size { get; set; }
        public string? Description { get; set; }
        public int CategoryId { get; set; }
        public decimal? Price { get; set; }
        public decimal? DailyFood { get; set; }
        public IFormFile? ImageUrl { get; set; }
    }
    public class UpdateFishDTO
    {
        public string? Name { get; set; }
        public int? Age { get; set; }
        public string? Gender { get; set; }
        public decimal? Size { get; set; }
        public string? Description { get; set; }
        public int? CategoryId { get; set; }
        public decimal? Price { get; set; }
        public decimal? DailyFood { get; set; }
        public IFormFile? ImageUrl { get; set; }
        public string? ProductStatus { get; set; }
    }
}
