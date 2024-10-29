using DataAccess.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IRepo
{

    public interface IFishRepo
    {
        Task<IEnumerable<Fish>> GetAllAsync();
        Task<List<Fish>> GetByCategoryIdAsync(int categoryId);
        Task<Fish> GetFishByIdAsync(int fishId);
        Task<bool> CategoryExists(int categoryId);
        Task AddFishAsync(Fish fish);
        Task UpdateFishAsync(Fish fish);
        Task DeleteFishAsync(Fish fish);
    }
}
