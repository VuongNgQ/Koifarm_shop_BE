﻿using DataAccess.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entity
{
    public class FishPackage
    {
        public int FishPackageId { get; set; }
        public string? Name { get; set; }
        public int? Age { get; set; }
        public decimal? MinSize { get; set; }
        public decimal? MaxSize { get; set; }
        public string? Description { get; set; }
        public decimal? TotalPrice { get; set; }
        public decimal? DailyFood { get; set; }
        public string? ImageUrl { get; set; }
        public int? NumberOfFish { get; set; }
        public int? Capacity { get; set; }
        public int? QuantityInStock { get; set; }
        public ProductStatusEnum ProductStatus { get; set; }
        public ICollection<OrderItem>? OrderItems { get; set; }
        public ICollection<CartItem>? CartItems { get; set; }
        public ICollection<SubImage> SubImages { get; set; }
        public ICollection<CategoryPackage> CategoryPackages { get; set; }
    }
}
