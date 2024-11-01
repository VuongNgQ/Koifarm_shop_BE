using DataAccess.Entity;
using DataAccess.IRepo;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repo
{
    public class OrderItemRepo:BaseRepo<OrderItem>, IOrderItemRepo
    {
        private readonly KoiShopContext _context;
        public OrderItemRepo(KoiShopContext context):base(context) 
        {
            _context = context;
        }

        public async Task<IEnumerable<OrderItem>> GetItemsByOrderId(int orderId)
        {
            return await _context.OrderItems.Where(c => c.OrderId == orderId).ToListAsync();
        }
    }
}
