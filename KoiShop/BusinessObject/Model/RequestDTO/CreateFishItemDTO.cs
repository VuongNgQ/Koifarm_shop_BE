﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Model.RequestDTO
{
    public class CreateFishItemDTO
    {
        public int UserCartId { get; set; }
        public int FishId { get; set; }
    }
}
