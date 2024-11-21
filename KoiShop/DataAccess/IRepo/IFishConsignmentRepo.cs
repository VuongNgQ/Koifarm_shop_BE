using DataAccess.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IRepo
{
    public interface IFishConsignmentRepo
    {
        Task<FishConsignment?> GetFishConsignmentByIdAsync(int id);
        Task<IEnumerable<FishConsignment>> GetAllFishConsignmentAsync();
        Task<IEnumerable<FishConsignment>> GetConsignmentsByUserIdAsync(int userId);
        Task<FishConsignment?> GetConsignmentByFishIdAsync(int fishId);
        Task<FishConsignment?> AddFishConsignmentAsync(FishConsignment consignment);
        Task<FishConsignment?> UpdateFishConsignmentAsync(FishConsignment consignment);
        Task DeleteFishConsignmentAsync(int id);
    }
}
