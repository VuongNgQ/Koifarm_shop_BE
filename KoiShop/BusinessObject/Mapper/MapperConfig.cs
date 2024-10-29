﻿using System;
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
            CreateMap<User, ResponseUserDTO>()
                .ForMember(u => u.RoleName, w => w.MapFrom(src => src.Role != null ? src.Role.RoleName : null))
                .ForMember(u=>u.Addresses, w=>w.MapFrom(src=>src.UserAddresses.Select(ua => ua.Address)))
                .ReverseMap();
            CreateMap<User, UserResponseDTO>().ReverseMap();
            CreateMap<User, UpdateUserDTO> ().ReverseMap();
            CreateMap<User, UpdateProfileDTO>().ReverseMap();
            //Cart
            CreateMap<UserCart, ResponseCartDTO>()
                .ForMember(c=>c.CartItems, i=>i.MapFrom(src=>src.CartItems))
                .ForMember(u=>u.UserName, c=>c.MapFrom(src=>src.User.Name))
                .ReverseMap();
            //Role Mapping
            CreateMap<Role, CreateRoleDTO>().ReverseMap();
            //Package Mapping
            CreateMap<FishPackage, CreateFishPackageDTO>().ReverseMap();
            CreateMap<FishPackage, ResponseFishPackageDTO>().ReverseMap();
            CreateMap<FishPackage, UpdatePackageDTO>().ReverseMap();
            //Category
            CreateMap<Category, CreateCategoryDTO>().ReverseMap();
            CreateMap<Category, ResponseCategoryDTO>().ReverseMap();
            CreateMap<Category, UpdateCategoryDTO>().ReverseMap();
            //Fish
            CreateMap<Fish, CreateFishDTO>().ReverseMap();
            CreateMap<Fish, ResponseFishDTO>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : null))
                .ReverseMap();
            CreateMap<Fish, UpdateFishDTO>().ReverseMap();
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
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Name))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.User.UserId))
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.OrderItems))
                .ReverseMap();
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
            CreateMap<OrderItem, CreateOrderItemDTO>().ReverseMap();
            CreateMap<OrderItem, ResponseOrderItemDTO>().ReverseMap();
            //Sub Image
            CreateMap<SubImage, CreateFishSubImageDTO>().ReverseMap();
            CreateMap<SubImage, CreatePackageSubImageDTO>().ReverseMap();
            CreateMap<SubImage, ResponseSubImageDTO>().ReverseMap();
        }
    }
}
