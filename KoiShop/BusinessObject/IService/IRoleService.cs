using BusinessObject.Model.RequestDTO;
using DataAccess.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.IService
{
    public interface IRoleService
    {
        Task<ServiceResponseFormat<CreateRoleDTO>> CreateRole(CreateRoleDTO role);
        Task<ServiceResponseFormat<IEnumerable<Role>>> GetRoles();
    }
}
