using AutoMapper;
using BusinessObject.IService;
using BusinessObject.Model.RequestDTO;
using DataAccess.Entity;
using DataAccess.IRepo;
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
        public async Task<ServiceResponseFormat<CreateUserDTO>> CreateUser(CreateUserDTO userDTO)
        {
            var res=new ServiceResponseFormat<CreateUserDTO>();
            try
            {
                var mapp=_mapper.Map<User>(userDTO);
                await _userRepo.CreateUser(mapp);
                res.Success = true;
                res.Message = "User created successfully";
                res.Data = userDTO;
                return res;
            }
            catch (Exception ex) 
            {
                res.Success = false;
                res.Message = $"Fail to create User:{ex.Message}";
            }
            return res;
        }

        public async Task<ServiceResponseFormat<IEnumerable<CreateUserDTO>>> GetAllUser()
        {
            var res = new ServiceResponseFormat<IEnumerable<CreateUserDTO>>();
            try
            {
                var users = await _userRepo.GetAllUser();
                var mapp = _mapper.Map<IEnumerable<CreateUserDTO>>(users);
                res.Success = true;
                res.Message = "User created successfully";
                res.Data = mapp;
                return res;
            }
            catch (Exception ex) 
            {
                res.Success = false;
                res.Message = $"Fail to get User:{ex.Message}";
            }
            return res;
        }
    }
}
