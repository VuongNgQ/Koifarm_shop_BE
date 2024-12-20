﻿using DataAccess.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IRepo
{
    public interface IOrderItemRepo:IBaseRepo<OrderItem>
    {
        Task<IEnumerable<OrderItem>> GetItemsByOrderId(int orderId);
    }
}
