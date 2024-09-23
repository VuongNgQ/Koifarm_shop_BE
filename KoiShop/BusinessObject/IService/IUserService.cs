using BusinessObject.Model.RequestDTO;
using DataAccess.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.IService
{
    public interface IUserService
    {
        Task<CreateUserDTO> CreateUser(CreateUserDTO createUserDTO);
        Task<IEnumerable<CreateUserDTO>> GetAllUser();
    }
}
