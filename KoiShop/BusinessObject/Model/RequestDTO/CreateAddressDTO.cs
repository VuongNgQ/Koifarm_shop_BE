﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Model.RequestDTO
{
    public class CreateAddressDTO
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string District { get; set; }
    }
}
