using BusinessObject.Model.RequestDTO;
using BusinessObject.Model.ResponseDTO;
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
        Task<ServiceResponseFormat<ResponseUserDTO>> CreateUser(CreateUserDTO createUserDTO);
        Task<ServiceResponseFormat<ResponseUserDTO>> CreateStaff(CreateUserDTO createUserDTO);
        Task <ServiceResponseFormat<PaginationModel<ResponseUserDTO>>> GetAllUser(int page, int pageSize,
            string search, string sort);
        Task<ServiceResponseFormat<ResponseUserDTO>> UpdateUser(int id, ResponseUserDTO updateUserDTO);
        Task<ServiceResponseFormat<UpdateProfileDTO>> UpdateProfile(int id, UpdateProfileDTO updateProfileDTO);
        Task<ServiceResponseFormat<bool>> RemoveUser(int id);
        Task<ServiceResponseFormat<bool>> DeleteUser(int id);
        Task<ServiceResponseFormat<bool>> RestoreUser(int id);
        Task<ServiceResponseFormat<ResponseUserDTO>> LoginUser(string email, string pass);
        //Task<ServiceResponseFormat<bool>> LoginAdmin(string email, string pass);
        //Task<ServiceResponseFormat<bool>> LoginCustomer(string email, string pass);
        //Task<ServiceResponseFormat<bool>> LoginStaff(string email, string pass);
    }
}
