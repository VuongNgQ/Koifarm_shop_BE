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
using DataAccess.Enum;
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
                .ForMember(u => u.Orders, w => w.MapFrom(src => src.Orders))
                .ForMember(u => u.UserCartId, c => c.MapFrom(src => src.UserCart.UserCartId))
                .ReverseMap();
            CreateMap<Order, UserOrder>().ReverseMap();
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
            CreateMap<FishPackage, CreateFishPackageDTO>()
                .ReverseMap();
            CreateMap<FishPackage, ResponseFishPackageDTO>()
                .ForMember(f=>f.Categories, o=>o.MapFrom(s=>s.CategoryPackages))
                .ReverseMap();
            CreateMap<FishPackage, UpdatePackageDTO>().ReverseMap();
            //Category Package
            CreateMap<CategoryPackage, CreateCategoryPackageDTO>().ReverseMap();
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
            
            //Addresss
            CreateMap<Address, CreateAddressDTO>().ReverseMap();
            CreateMap<Address, ResponseAddressDTO>().ReverseMap();
            CreateMap<Address, UpdateAddressDTO>().ReverseMap();
            //Order 
            CreateMap<Order, CreateOrderDTO>()
                .ForMember(dest => dest.CreateAddressDTO, opt => opt.MapFrom(src => src.Address))
                .ReverseMap();
            CreateMap<Order, ResponseOrderDTO>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Name))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.User.UserId))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.User.Phone))
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.OrderItems))
                .ReverseMap();
            CreateMap<Order, UpdateOrderDTO>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ReverseMap();
            
            //Cart
            CreateMap<UserCart, CreateCartDTO>().ReverseMap();
            //Item
            CreateMap<CartItem, CreateFishItemDTO>().ReverseMap();
            CreateMap<CartItem, CreatePackageItemDTO>().ReverseMap();
            CreateMap<CartItem, ResponseCartItemDTO>()
                .ForMember(dest => dest.FishName, opt => opt.MapFrom(src => src.Fish.Name))
                .ForMember(dest => dest.FishImage, opt => opt.MapFrom(src => src.Fish.ImageUrl))
                .ForMember(dest => dest.PackageImage, opt => opt.MapFrom(src => src.Package.ImageUrl))
                .ForMember(dest => dest.PackageName, opt => opt.MapFrom(src => src.Package.Name))
                .ReverseMap();

            CreateMap<OrderItem, CreateFishItemDTO>().ReverseMap();
            CreateMap<OrderItem, CreateOrderItemDTO>().ReverseMap();
            CreateMap<OrderItem, ResponseOrderItemDTO>()
                .ForMember(dest => dest.FishName, opt => opt.MapFrom(src => src.Fish.Name))
                .ForMember(dest => dest.FishImage, opt => opt.MapFrom(src => src.Fish.ImageUrl))
                .ForMember(dest => dest.PackageImage, opt => opt.MapFrom(src => src.Package.ImageUrl))
                .ForMember(dest => dest.PackageName, opt => opt.MapFrom(src => src.Package.Name))
                .ReverseMap();
            //Sub Image
            CreateMap<SubImage, CreateFishSubImageDTO>().ReverseMap();
            CreateMap<SubImage, CreatePackageSubImageDTO>().ReverseMap();
            CreateMap<SubImage, ResponseSubImageDTO>().ReverseMap();
            //Fish Consignment
            CreateMap<FishConsignment, FishConsignmentDTO>().ReverseMap();
            CreateMap<FishConsignment, CreateConsignmentDTO>().ReverseMap();
            // Trong MapperConfig, thêm mapping cho Payment
            CreateMap<Payment, PaymentDTO>()
                .ForMember(dest => dest.TransactionId, opt => opt.MapFrom(src => src.TransactionId))
                .ForMember(dest => dest.RelatedId, opt => opt.MapFrom(src =>
                    src.TransactionType == TransactionPurpose.BuyFish ? src.OrderId : src.FishConsignmentId))
                .ForMember(dest => dest.CreateDate, opt => opt.MapFrom(src => src.CreatedDate ?? DateTime.MinValue))
                .ForMember(dest => dest.PaymentDate, opt => opt.MapFrom(src => src.PaymentDate ?? DateTime.MinValue))
                .ForMember(dest => dest.Method, opt => opt.MapFrom(src => src.Order.PaymentMethod))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.PaymentStatus))
                .ReverseMap();
            CreateMap<Payment, CreatePaymentDTO>().ReverseMap();
            CreateMap<Payment, UpdatePaymentDTO>().ReverseMap();
        }
    }
}
