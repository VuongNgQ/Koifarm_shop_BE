﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Model.ResponseDTO
{
    public class ResponseCartItemDTO
    {
        public int CartItemId { get; set; }
        public int UserCartId { get; set; }
        public int? FishId { get; set; }
        public string? FishName { get; set; }
        public string? FishImage {  get; set; }
        public int? PackageId { get; set; }
        public string? PackageName { get; set; }
        public string? PackageImage {  get; set; }
        public int? Quantity { get; set; }
        public decimal? TotalPricePerItem { get; set; }
        public string CartItemStatus { get; set; }
    }
}
