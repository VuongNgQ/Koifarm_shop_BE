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
        private readonly KoiShopContext _context;
        public RoleRepo(KoiShopContext context):base(context) 
        {
            _context = context;
        }

        public async Task<Role> CreateRole(Role role)
        {
            await _context.Set<Role>().AddAsync(role);
            await _context.SaveChangesAsync();
            return role;
        }

        public async Task<IEnumerable<Role>> GetAllRoles()
        {
            return await _context.Set<Role>().ToListAsync();
        }

        public async Task<bool> RoleExist(int id)
        {
            return await _context.Set<Role>().AnyAsync(r => r.RoleId == id);
        }
    }
}
