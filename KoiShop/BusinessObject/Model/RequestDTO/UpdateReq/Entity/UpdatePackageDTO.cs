using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Model.RequestDTO.UpdateReq.Entity
{
    public class UpdatePackageDTO
    {
        public string? Name { get; set; }
        public int? Age { get; set; }
        public string? Gender { get; set; }
        public decimal? Size { get; set; }
        public string? Description { get; set; }

        public decimal? TotalPrice { get; set; }
        public decimal? DailyFood { get; set; }
        // Use IFormFile for local file uploads
        public IFormFile? ImageFile { get; set; }
        public int? NumberOfFish { get; set; }

        public string? Status { get; set; }
    }
}
