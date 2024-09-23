using AutoMapper;
using BusinessObject.IService;
using BusinessObject.Model.RequestDTO;
using DataAccess.Entity;
using DataAccess.IRepo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Service
{
    public class RoleService:IRoleService
    {
        private readonly IRoleRepo _roleRepo;
        private readonly IMapper _mapper;
        public RoleService(IRoleRepo repo, IMapper mapper)
        {
            _roleRepo = repo;
            _mapper = mapper;
        }

        public async Task<ServiceResponseFormat<CreateRoleDTO>> CreateRole(CreateRoleDTO role)
        {
            var res=new ServiceResponseFormat<CreateRoleDTO>();
            try
            {
                var mapp=_mapper.Map<Role>(role);
                await _roleRepo.CreateRole(mapp);
                res.Success = true;
                res.Message = "Role created successfully";
                res.Data = role;
                return res;
            }
            catch(Exception ex)
            {
                res.Success = false;
                res.Message = $"Fail to create Role: {ex.Message}";
            }
            return res;
        }

        public async Task<ServiceResponseFormat<IEnumerable<Role>>> GetRoles()
        {
            var res = new ServiceResponseFormat<IEnumerable<Role>>();
            try
            {
                var roles= await _roleRepo.GetAllRoles();
                res.Success = true;
                res.Message = "Role retrieve successfully";
                res.Data = roles;
                return res;
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = $"Fail to get Role: {ex.Message}";
            }
            return res;
        }
    }
}
