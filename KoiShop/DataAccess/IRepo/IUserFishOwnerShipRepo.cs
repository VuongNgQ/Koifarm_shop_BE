using DataAccess.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IRepo
{
    public interface IUserFishOwnerShipRepo : IBaseRepo<UserFishOwnership>
    {
        Task<UserFishOwnership?> GetOwnershipByUserAndFishAsync(int userId, int fishId);
    }
}
