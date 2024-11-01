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
    public class CartItemRepo:BaseRepo<CartItem>, ICartItemRepo
    {
        private readonly KoiShopContext _context;
        public CartItemRepo(KoiShopContext context):base(context) 
        {
            _context = context;
        }

        public async Task<IEnumerable<CartItem>> GetAll()
        {
            return await _context.Set<CartItem>().Include(c => c.Fish)
                .Include(ci => ci.Package)
                .ToListAsync();
        }
    }
}
