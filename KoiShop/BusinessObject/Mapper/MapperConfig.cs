using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BusinessObject.Model.RequestDTO;
using DataAccess.Entity;
namespace BusinessObject.Mapper
{
    public class MapperConfig:Profile
    {
        public MapperConfig()
        {
            //User Mapping
            CreateMap<User, CreateUserDTO>().ReverseMap();
        }
    }
}
