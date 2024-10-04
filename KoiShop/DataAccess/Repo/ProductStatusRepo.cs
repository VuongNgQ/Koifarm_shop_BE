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
    public class ProductStatusRepo:BaseRepo<ProductStatus>, IProductStatusRepo
    {
        private readonly KoiShopContext _context;
        public ProductStatusRepo(KoiShopContext context):base(context) 
        {
            _context = context;
        }

        public async Task<ProductStatus> CreateFishStatus(ProductStatus status)
        {
            await _context.Set<ProductStatus>().AddAsync(status);
            await _context.SaveChangesAsync();
            return status;
        }

        public async Task<bool> DeleteFishStatus(int id)
        {
            var exist = await _context.Set<ProductStatus>().FirstOrDefaultAsync(s => s.ProductStatusId == id);
            if (exist != null)
            {
                _context.Remove(exist);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteStatusByName(string name)
        {
            var exist = await _context.Set<ProductStatus>().FirstOrDefaultAsync(s => s.Name == name);
            if (exist != null)
            {
                _context.Remove(exist);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<ProductStatus>> GetFishStatuses()
        {
            return await _context.Set<ProductStatus>().ToListAsync();
        }

        public async Task<ProductStatus> GetStatusById(int id)
        {
            return await _context.Set<ProductStatus>().FirstOrDefaultAsync(s => s.ProductStatusId == id);
        }

        public async Task<ProductStatus> GetStatusByName(string name)
        {
            return await _context.Set<ProductStatus>().FirstOrDefaultAsync(s => s.Name.Contains(name.ToLower().Trim()));
        }
    }
}
