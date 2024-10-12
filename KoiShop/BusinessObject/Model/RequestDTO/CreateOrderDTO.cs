﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Model.RequestDTO
{
    public class CreateOrderDTO
    {
        public int UserId { get; set; }
        public DateTime? OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public int StatusId { get; set; }
        public int PaymentMethodId { get; set; }
        public bool IsSent { get; set; }
        public CreateAddressDTO CreateAddressDTO { get; set; }
    }
}
