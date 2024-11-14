using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Model.RequestDTO.UpdateReq.Entity
{
    public class UpdatePackageDTO
    {
        public string? Name { get; set; }
        public int? Age { get; set; }
        public decimal? MinSize { get; set; }
        public decimal? MaxSize { get; set; }
        public string? Description { get; set; }
        public decimal? TotalPrice { get; set; }
        public decimal? DailyFood { get; set; }
        public IFormFile? ImageUrl { get; set; }
        public int? Capacity { get; set; }
        public int? QuantityInStock { get; set; }
        public string? ProductStatus { get; set; }
    }
}
