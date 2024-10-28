using DataAccess.Entity;
using DataAccess.IRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repo
{
    public class FishRepository : IFishRepo
    {
        private readonly KoiShopContext _context;

        public FishRepository(KoiShopContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Fish>> GetAllAsync()
        {
            return await _context.Fish.Include(f => f.Category).ToListAsync();
        }

        public async Task<Fish?> GetFishByIdAsync(int fishId)
        {
            return await _context.Fish.Include(f => f.Category).FirstOrDefaultAsync(f => f.FishId == fishId);
        }
        public async Task<bool> CategoryExists(int categoryId)
        {
            return await _context.Categories.AnyAsync(c => c.CategoryId == categoryId);
        }
        public async Task AddFishAsync(Fish fish)
        {
            await _context.Fish.AddAsync(fish);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateFishAsync(Fish fish)
        {
            _context.Fish.Update(fish);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteFishAsync(Fish fish)
        {
            _context.Fish.Remove(fish);
            await _context.SaveChangesAsync();
        }
    }


}
