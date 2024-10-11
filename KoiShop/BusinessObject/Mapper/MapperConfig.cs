using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BusinessObject.Model.RequestDTO;
using BusinessObject.Model.RequestDTO.UpdateReq.Entity;
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
            CreateMap<FishStatus, CreateFishStatusDTO>().ReverseMap();
            CreateMap<FishStatus, ResponseFishStatusDTO>().ReverseMap();
            CreateMap<FishStatus, UpdateFishStatusDTO>().ReverseMap();
            //Product Status Mapping
            CreateMap<ProductStatus, CreateProductStatusDTO>().ReverseMap();
            CreateMap<ProductStatus, ResponseProductStatusDTO>().ReverseMap();
            //Consignment Type Mapping
            CreateMap<ConsignmentType, CreateConsignmentTypeDTO>().ReverseMap();
            CreateMap<ConsignmentType, ResponseConsignmentTypeDTO>().ReverseMap();
        }
    }
}
