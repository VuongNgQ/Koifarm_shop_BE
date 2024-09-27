using DataAccess.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.IService
{
    public interface IFishPackageService
    {
        Task<ServiceResponseFormat<IEnumerable<FishPackage>>> GetFishPackages();
        Task<ServiceResponseFormat<FishPackage>> GetFishPackage(int id);
        Task<ServiceResponseFormat<FishPackage>> CreatePackage(FishPackage package);
        Task<ServiceResponseFormat<FishPackage>> UpdatePackage(int id, FishPackage fishPackage);
        Task<ServiceResponseFormat<bool>> DeletePackage(int id);
    }
}
