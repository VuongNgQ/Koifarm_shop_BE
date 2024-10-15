using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Model.ResponseDTO
{
    public class ResponseCartItemDTO
    {
        public int CartItemId { get; set; }
        public int CartId { get; set; }
        public int? FishId { get; set; }
        public int? PackageId { get; set; }
        public int? Quantity { get; set; }
        public decimal? TotalPricePerItem { get; set; }
    }
}
