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
        //Task<int> CreateConsignmentAsync(FishConsignment consignment);
        Task AddAsync(FishConsignment consignment);
        Task<FishConsignment?> GetConsignmentByIdAsync(int consignmentId);
        Task<List<FishConsignment>> GetAllConsignmentsAsync();
        Task UpdateConsignmentAsync(FishConsignment consignment);
    }
}
