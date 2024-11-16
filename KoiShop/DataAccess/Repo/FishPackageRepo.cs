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
    public class FishPackageRepo:BaseRepo<FishPackage>, IFishPackageRepo
    {
        private readonly KoiShopContext _context;
        public FishPackageRepo(KoiShopContext context):base(context) 
        {
            _context = context;
        }

        public async Task<FishPackage> CreatePackage(FishPackage package)
        {
            await _context.Set<FishPackage>().AddAsync(package);
            await _context.SaveChangesAsync();
            return package;
        }

        public async Task<bool> DeletePackage(int id)
        {
            var exist=await _context.Set<FishPackage>().FirstOrDefaultAsync(f=>f.FishPackageId==id);
            if (exist != null)
            {
                _context.Remove(exist);
                await _context.SaveChangesAsync();
                return true;
            }
            else { return false; }
        }

        public async Task<FishPackage> GetFishPackage(int id)
        {
            return await _context.Set<FishPackage>().Include(p=>p.CategoryPackages).FirstOrDefaultAsync(p => p.FishPackageId == id);
        }

        public async Task<IEnumerable<FishPackage>> GetFishPackages()
        {
            return await _context.Set<FishPackage>().Include(p=>p.CategoryPackages).ToListAsync();
        }

        public async Task<FishPackage> UpdatePackage(int id, FishPackage fishPackage)
        {
            var exist = await _context.Set<FishPackage>().FirstOrDefaultAsync(p => p.FishPackageId == id);
            if(exist != null)
            {
                exist.Name = fishPackage.Name;
                exist.Description = fishPackage.Description;
                exist.NumberOfFish= fishPackage.NumberOfFish;
                exist.DailyFood = fishPackage.DailyFood;
                exist.ImageUrl = fishPackage.ImageUrl;
                exist.ProductStatus = fishPackage.ProductStatus;
                exist.TotalPrice = fishPackage.TotalPrice;
                
                _context.Update(exist);
                await _context.SaveChangesAsync();
                return exist;
            }
            else
            {
                throw new Exception("Package Not Found");
            }
        }
    }
}
