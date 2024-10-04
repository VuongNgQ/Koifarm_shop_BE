using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BusinessObject.Model.RequestDTO;
using BusinessObject.Model.ResponseDTO;
using DataAccess.Entity;
namespace BusinessObject.Mapper
{
    public class MapperConfig:Profile
    {
        public MapperConfig()
        {
            //User Mapping
            CreateMap<User, CreateUserDTO>().ReverseMap();
            CreateMap<User, ResponseUserDTO>().ReverseMap();
            //Role Mapping
            CreateMap<Role, CreateRoleDTO>().ReverseMap();
            //Package Mapping
            CreateMap<FishPackage, CreateFishPackageDTO>().ReverseMap();
            CreateMap<FishPackage, ResponseFishPackageDTO>().ReverseMap();
            //Fish Status Mapping
            CreateMap<ProductStatus, CreateFishStatusDTO>().ReverseMap();
            CreateMap<ProductStatus, ResponseFishStatusDTO>().ReverseMap();
        }
    }
}
