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
        Task<IEnumerable<Fish>> GetAllFishesAsync();
        Task<Fish> GetFishByIdAsync(int fishId);
        Task AddFishAsync(Fish fish);
        Task UpdateFishAsync(Fish fish);
        Task DeleteFishAsync(int fishId);
    }
}
