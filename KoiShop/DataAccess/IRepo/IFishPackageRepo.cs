using DataAccess.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IRepo
{
    public interface IFishPackageRepo:IBaseRepo<FishPackage>
    {
        Task<IEnumerable<FishPackage>> GetFishPackages();
        Task<FishPackage> GetFishPackage(int id);
        Task<FishPackage> CreatePackage(FishPackage package);
        Task<FishPackage> UpdatePackage(int id, FishPackage fishPackage);
        Task<bool> DeletePackage(int id);
    }
}
