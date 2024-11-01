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
    public class CartRepo:BaseRepo<UserCart>, ICartRepo
    {
        private readonly KoiShopContext _context;
        public CartRepo(KoiShopContext context):base(context) 
        {
            _context = context;
        }

        public async Task<IEnumerable<UserCart>> GetAll()
        {
            return await _context.Set<UserCart>().Include(c=>c.User).Include(c=>c.CartItems).ThenInclude(c=>c.Fish)
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Package)
                .ToListAsync();
        }
    }
}
