using DataAccess.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IRepo
{
    public interface IRoleRepo:IBaseRepo<Role>
    {
        Task<Role> CreateRole(Role role);
        Task<IEnumerable<Role>> GetAllRoles();
    }
}
