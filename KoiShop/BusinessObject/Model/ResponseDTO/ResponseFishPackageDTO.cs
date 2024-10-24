﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Model.ResponseDTO
{
    public class ResponseFishPackageDTO
    {
        public int FishPackageId { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public decimal Size { get; set; }
        public string Description { get; set; }
       
        public decimal TotalPrice { get; set; }
        public decimal DailyFood { get; set; }
        public string ImageUrl { get; set; }
        public int NumberOfFish { get; set; }
        
        public string Status { get; set; }
    }
}
