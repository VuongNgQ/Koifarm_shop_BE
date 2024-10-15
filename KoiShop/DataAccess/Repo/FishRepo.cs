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

        public async Task<IEnumerable<Fish>> GetAllFishesAsync()
        {
            return await _context.Fish.Include(f => f.Category).ToListAsync();
        }

        public async Task<Fish?> GetFishByIdAsync(int fishId)
        {
            var fish = await _context.Fish.FirstOrDefaultAsync(f => f.FishId == fishId);
            if (fish == null)
            {
                throw new KeyNotFoundException($"Fish with ID {fishId} not found.");
            }
            return fish;
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

        public async Task DeleteFishAsync(int fishId)
        {
            var fish = await _context.Fish.FindAsync(fishId);
            if (fish != null)
            {
                _context.Fish.Remove(fish);
                await _context.SaveChangesAsync();
            }else
            {
                throw new KeyNotFoundException("Fish not found");
            }
        }
    }


}
