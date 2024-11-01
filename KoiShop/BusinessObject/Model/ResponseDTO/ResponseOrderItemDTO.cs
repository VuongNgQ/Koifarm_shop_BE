using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Model.ResponseDTO
{
    public class ResponseOrderItemDTO
    {
        public int OrderItemId { get; set; }
        public int OrderId { get; set; }
        public int? FishId { get; set; }
        public string? FishName { get; set; }
        public string? FishImage { get; set; }
        public int? PackageId { get; set; }
        public string? PackageName { get; set; }
        public string? PackageImage {  get; set; }
        public int? Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
