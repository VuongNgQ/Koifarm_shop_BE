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
    public class UserFishOwnerShipRepo : BaseRepo<UserFishOwnership>, IUserFishOwnerShipRepo
    {
        private readonly KoiShopContext _context;
        public UserFishOwnerShipRepo(KoiShopContext context) : base(context)
        {
            _context = context;
        }
        public async Task<UserFishOwnership?> GetOwnershipByUserAndFishAsync(int userId, int fishId)
        {
            return await _context.Set<UserFishOwnership>()
                                 .FirstOrDefaultAsync(o => o.UserId == userId && o.FishId == fishId);
        }
    }
}
