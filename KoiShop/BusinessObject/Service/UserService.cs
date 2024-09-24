using AutoMapper;
using BusinessObject.IService;
using BusinessObject.Model.RequestDTO;
using BusinessObject.Model.ResponseDTO;
using BusinessObject.Utils;
using DataAccess.Entity;
using DataAccess.IRepo;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepo _userRepo;
        private readonly IMapper _mapper;
        public UserService(IUserRepo repo, IMapper mapper)
        {
            _userRepo = repo;
            _mapper = mapper;
        }
        public async Task<ServiceResponseFormat<ResponseUserDTO>> CreateUser(CreateUserDTO userDTO)
        {
            var res=new ServiceResponseFormat<ResponseUserDTO>();
            try
            {
                var mapp=_mapper.Map<User>(userDTO);
                mapp.Status = "Active";
                await _userRepo.CreateUser(mapp);
                var result = _mapper.Map<ResponseUserDTO>(mapp);
                res.Success = true;
                res.Message = "User created successfully";
                res.Data = result;
                return res;
            }
            catch (Exception ex) 
            {
                res.Success = false;
                res.Message = $"Fail to create User:{ex.Message}";
            }
            return res;
        }

        public async Task<ServiceResponseFormat<bool>> DeleteUser(int id)
        {
            var res = new ServiceResponseFormat<bool>();
            try
            {
                var result=await _userRepo.DeleteUser(id);
                if (result)
                {
                    res.Success = true;
                    res.Message = "User Deleted successfully";
                    return res;
                }
                else
                {
                    res.Success = false;
                    res.Message = "No User found";
                    return res;
                }
            }
            catch(Exception ex)
            {
                res.Success = false;
                res.Message = $"Fail to delete User:{ex.Message}";
                return res;
            }
        }

        public async Task<ServiceResponseFormat<PaginationModel<ResponseUserDTO>>> GetAllUser(int page, int pageSize,
            string? search, string sort)
        {
            var res = new ServiceResponseFormat<PaginationModel<ResponseUserDTO>>();
            try
            {
                var users = await _userRepo.GetAllUser();
                if (!string.IsNullOrEmpty(search))
                {
                    users = users.Where(e => e.Name.Contains(search, StringComparison.OrdinalIgnoreCase)||
                    e.Email.Contains(search, StringComparison.OrdinalIgnoreCase)||
                    e.Phone.Contains(search, StringComparison.OrdinalIgnoreCase));
                }
                users = sort.ToLower().Trim() switch
                {
                    "name" => users.OrderBy(e => e.Name),
                    _ => users.OrderBy(e => e.UserId)
                };
                var mapp = _mapper.Map<IEnumerable<ResponseUserDTO>>(users);
                if (mapp.Any())
                {
                    var paginationModel = await Pagination.GetPaginationEnum(mapp, page, pageSize);
                    res.Success = true;
                    res.Message = "Get Users successfully";
                    res.Data = paginationModel;
                    return res;
                }
                else
                {
                    res.Success = false;
                    res.Message = "No User";
                    return res;
                }
            }
            catch (Exception ex) 
            {
                res.Success = false;
                res.Message = $"Fail to get User:{ex.Message}";
            }
            return res;
        }

        public async Task<ServiceResponseFormat<bool>> LoginUser(string email, string pass)
        {
            var res = new ServiceResponseFormat<bool>();
            try
            {
                var result=await _userRepo.Login(email, pass);
                if (result)
                {
                    res.Success = true;
                    res.Message = "Login Successfully";
                    return res;
                }
                else
                {
                    res.Success = false;
                    res.Message = "Password does not match the Email";
                    return res;
                }
            }
            catch(Exception ex)
            {
                res.Success = false;
                res.Message = $"Login fail:{ex.Message}";
                return res;
            }
        }

        public async Task<ServiceResponseFormat<ResponseUserDTO>> UpdateUser(int id, ResponseUserDTO updateUserDTO)
        {
            var res = new ServiceResponseFormat<ResponseUserDTO>();
            try
            {
                var mapp = _mapper.Map<User>(updateUserDTO);
                var updateUser = await _userRepo.UpdateUser(id, mapp);
                if(updateUser != null)
                {
                    res.Success = true;
                    res.Message = "User Updated Successfully";
                    res.Data = updateUserDTO;
                    return res;
                }
                else
                {
                    res.Success = false;
                    res.Message = "No User Found or got error at Repo";
                    return res;
                }
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = $"Fail to update User:{ex.Message}";
                return res;
            }
        }
    }
}
