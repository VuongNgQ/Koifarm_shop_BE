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
            CreateMap<User, UpdateProfileDTO>().ReverseMap();
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
            //Addresss
            CreateMap<Address, CreateAddressDTO>().ReverseMap();
            CreateMap<Address, ResponseAddressDTO>().ReverseMap();
            //Order 
            CreateMap<Order, CreateOrderDTO>()
                .ForMember(dest => dest.CreateAddressDTO, opt => opt.MapFrom(src => src.Address))
                .ReverseMap();
            CreateMap<Order, ResponseOrderDTO>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User))
                .ReverseMap();
            //Order Status
            CreateMap<OrderStatus, CreateOrderStatusDTO>().ReverseMap();
            CreateMap<OrderStatus, ResponseOrderStatusDTO>().ReverseMap();
            //Payment Method
            CreateMap<PaymentMethod, CreatePaymentMethodDTO>().ReverseMap();
            CreateMap<PaymentMethod, ResponsePaymentMethodDTO>().ReverseMap();
            //Cart
            CreateMap<UserCart, CreateCartDTO>().ReverseMap();
            //Item
            CreateMap<CartItem, CreateFishItemDTO>().ReverseMap();
            CreateMap<CartItem, CreatePackageItemDTO>().ReverseMap();
            CreateMap<CartItem, ResponseCartItemDTO>().ReverseMap();

            CreateMap<OrderItem, CreateFishItemDTO>().ReverseMap();
            CreateMap<OrderItem, CreateOrderPackageItemDTO>().ReverseMap();
            CreateMap<OrderItem, ResponseOrderItemDTO>().ReverseMap();
        }
    }
}
