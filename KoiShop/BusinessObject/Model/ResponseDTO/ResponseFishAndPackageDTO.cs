using DataAccess.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Model.ResponseDTO
{
    public class ResponseFishAndPackageDTO
    {
        public int? FishId { get; set; }
        public int? FishPackageId { get; set; }
        public string? Name { get; set; }
        public decimal? Price { get; set; }
        public decimal? MinSize { get; set; }
        public decimal? MaxSize { get; set; }
        public int? NumberOfFish { get; set; }
        public string? ProductStatus { get; set; } // Changed to string
        public string? Description { get; set; }
        public bool IsPackage { get; set; }
    }


}
