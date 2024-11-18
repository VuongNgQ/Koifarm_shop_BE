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
    public class OrderRepo:BaseRepo<Order>, IOrderRepo
    {
        private readonly KoiShopContext _context;
        public OrderRepo(KoiShopContext context):base(context)
        {
            _context=context;
        }

        public async Task<IEnumerable<Order>> GetAllOrder()
        {
            return await _context.Set<Order>().Include(a=>a.Address).Include(u=>u.User).Include(o=>o.OrderItems).ThenInclude(c => c.Fish)
                .Include(c => c.OrderItems)
                .ThenInclude(ci => ci.Package).ToListAsync();
        }
        public async Task<Order> GetByIdWithItemsAsync(int id)
        {
            return await _context.Orders
                .Include(o => o.OrderItems) // Includes OrderItems for the specified Order
                .FirstOrDefaultAsync(o => o.OrderId == id);
        }
        public async Task<IEnumerable<Order>> GetOrdersByUserId(int userId)
        {
            return await _context.Orders
                .Where(o => o.UserId == userId) // Lọc theo UserId
                .Include(o => o.Address)        // Bao gồm Address
                .Include(o => o.User)           // Bao gồm User
                .Include(o => o.OrderItems)     // Bao gồm OrderItems
                .ThenInclude(oi => oi.Fish)     // Bao gồm Fish từ OrderItems
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Package)  // Bao gồm Package từ OrderItems
                .ToListAsync();
        }
        public async Task UpdateOrder(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }

    }
}
