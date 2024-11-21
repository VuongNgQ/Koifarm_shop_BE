using DataAccess.Entity;
using DataAccess.Enum;
using DataAccess.IRepo;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repo
{
    public class FishConsignmentRepo : BaseRepo<FishConsignment>, IFishConsignmentRepo
    {
        private readonly new KoiShopContext _context;
        public FishConsignmentRepo(KoiShopContext context) : base(context)
        {
            _context = context;
        }
        public async Task<FishConsignment?> GetFishConsignmentByIdAsync(int id)
        {
            return await _context.FishConsignments
                .Include(fc => fc.User)
                .Include(fc => fc.Fish)
                .ThenInclude(f => f.Category)
                .Include(fc => fc.Payments)
                .FirstOrDefaultAsync(fc => fc.FishConsignmentId == id);
        }
        public async Task<IEnumerable<FishConsignment>> GetAllFishConsignmentAsync()
        {
            return await _context.FishConsignments
                .Include(fc => fc.User)
                .Include(fc => fc.Fish)
                .ThenInclude(f => f.Category)
                .Include(fc => fc.Payments)
                .ToListAsync();
        }
        public async Task<IEnumerable<FishConsignment>> GetConsignmentsByUserIdAsync(int userId)
        {
            return await _context.FishConsignments
                .Where(c => c.UserId == userId)
                .Include(fc => fc.Fish).ThenInclude(f => f.Category)
                .Include(fc => fc.Payments)
                .ToListAsync();
        }
        public async Task<FishConsignment?> GetConsignmentByFishIdAsync(int fishId)
        {
            return await _context.FishConsignments
                .FirstOrDefaultAsync(c => c.FishId == fishId);
        }

        public async Task<FishConsignment?> AddFishConsignmentAsync(FishConsignment consignment)
        {
            await _context.FishConsignments.AddAsync(consignment);
            await _context.SaveChangesAsync();
            return await _context.FishConsignments.FirstOrDefaultAsync(f => f.FishConsignmentId ==consignment.FishConsignmentId);
        }

        public async Task<FishConsignment?> UpdateFishConsignmentAsync(FishConsignment consignment)
        {
            _context.FishConsignments.Update(consignment);
            await _context.SaveChangesAsync();
            return await _context.FishConsignments.FirstOrDefaultAsync(f => f.FishConsignmentId == consignment.FishConsignmentId);
        }

        public async Task DeleteFishConsignmentAsync(int id)
        {
            var consignment = await GetFishConsignmentByIdAsync(id);
            if (consignment != null)
            {
                _context.FishConsignments.Remove(consignment);
                await _context.SaveChangesAsync();
            }
        }
    }
}