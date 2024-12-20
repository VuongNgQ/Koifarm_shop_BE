﻿using DataAccess.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entity
{
    public class CartItem
    {
        public int CartItemId { get; set; }
        public int? UserCartId { get; set; }
        public int? FishId { get; set; }
        public int? PackageId { get; set; }
        public int? Quantity { get; set; }
        public decimal? TotalPricePerItem { get; set; }
        public CartItemStatus CartItemStatus { get; set; }
        public int? OrderId { get; set; }

        public Order Order { get; set; }
        public UserCart? UserCart { get; set; }
        public Fish? Fish { get; set; }
        public FishPackage? Package { get; set; }
    }
}
