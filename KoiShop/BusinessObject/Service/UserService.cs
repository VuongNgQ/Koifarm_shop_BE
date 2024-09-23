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
        public async Task<CreateUserDTO> CreateUser(CreateUserDTO userDTO)
        {
            try
            {
                var mapp=_mapper.Map<User>(userDTO);
                await _userRepo.CreateUser(mapp);
                return userDTO;
            }
            catch (Exception ex) 
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<CreateUserDTO>> GetAllUser()
        {
            var users= await _userRepo.GetAllUser();
            var mapp = _mapper.Map<IEnumerable<CreateUserDTO>>(users);
            return mapp;
        }
    }
}
