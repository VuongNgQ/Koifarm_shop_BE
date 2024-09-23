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
    public class RoleRepo:BaseRepo<Role>, IRoleRepo
    {
        private readonly KoiShopContext _dbContext;
        public RoleRepo(KoiShopContext context):base(context) 
        {
            _dbContext = context;
        }

        public async Task<Role> CreateRole(Role role)
        {
            await _dbContext.Set<Role>().AddAsync(role);
            await _dbContext.SaveChangesAsync();
            return role;
        }

        public async Task<IEnumerable<Role>> GetAllRoles()
        {
            return await _dbContext.Set<Role>().ToListAsync();
        }
    }
}
