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
    public class FishConsignmentRepo : BaseRepo<FishConsignment>, IFishConsignmentRepo
    {
        private readonly new KoiShopContext _context;
        public FishConsignmentRepo(KoiShopContext context) : base(context)
        {
            _context = context;
        }
        public async Task AddAsync(FishConsignment consignment)
        {
            await _context.FishConsignments.AddAsync(consignment);
            await _context.SaveChangesAsync();
        }

        public async Task<FishConsignment?> GetConsignmentByIdAsync(int consignmentId)
        {
            return await _context.FishConsignments.FindAsync(consignmentId);
        }

        public async Task<List<FishConsignment>> GetAllConsignmentsAsync()
        {
            return await _context.FishConsignments.ToListAsync();
        }
        public async Task UpdateConsignmentAsync(FishConsignment consignment)
        {
            var existingConsignment = await _context.FishConsignments.FindAsync(consignment.FishConsignmentId);
            if (existingConsignment == null)
                throw new KeyNotFoundException("Consignment not found.");

            _context.Entry(existingConsignment).CurrentValues.SetValues(consignment);

            await _context.SaveChangesAsync();
        }
    }
}