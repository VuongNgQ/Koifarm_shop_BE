using BusinessObject.Configuration;
using BusinessObject.IService;
using BusinessObject.Service;
using DataAccess;
using DataAccess.IRepo;
using DataAccess.Repo;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddService(this IServiceCollection services, string? DatabaseConnection)
        {
            services.AddScoped(typeof(IBaseRepo<>), typeof(BaseRepo<>));
            //User
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRepo, UserRepo>();
            //User Address
            services.AddScoped<IUserAddressRepo, UserAddressRepo>();
            //Role
            services.AddScoped<IRoleRepo, RoleRepo>();
            services.AddScoped<IRoleService, RoleService>();
            //Category
            services.AddScoped<ICategoryRepo, CategoryRepo>();
            services.AddScoped<ICategoryService, CategoryService>();
            //Fish Package
            services.AddScoped<IFishPackageService, FishPackageService>();
            services.AddScoped<IFishPackageRepo, FishPackageRepo>();
            //Consignment Type
            services.AddScoped<IConsignmentTypeRepo, ConsignmentTypeRepo>();
            services.AddScoped<IConsignmentTypeService, ConsignmentTypeService>();
            //Order
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IOrderRepo, OrderRepo>();
            //SubImage
            services.AddScoped<ISubImageRepo, SubImageRepo>();
            services.AddScoped<ISubImageService, SubImageService>();
            //Address
            services.AddScoped<IAddressRepo, AddressRepo>();
            //Payment Method
            services.AddScoped<IPaymentMethodRepo, PaymentMethodRepo>();
            services.AddScoped<IPaymentMethodService, PaymentMethodService>();
            //Fish 
            services.AddScoped<IFishRepo, FishRepository>();
            services.AddScoped<IFishService, FishService>();
            //Cart
            services.AddScoped<ICartRepo, CartRepo>();
            services.AddScoped<ICartService, CartService>();
            //Cart Item
            services.AddScoped<ICartItemRepo, CartItemRepo>();
            services.AddScoped<ICartItemService, CartItemService>();
            //Order Item
            services.AddScoped<IOrderItemRepo, OrderItemRepo>();
            services.AddScoped<IOrderItemService, OrderItemService>();
            //FishConsignment
            services.AddScoped<IFishConsignmentRepo, FishConsignmentRepo>();
            services.AddScoped<IFishConsignmentService, FishConsignmentService>();
            //Service
            services.AddScoped<IPaymentRepo, PaymentRepo>();
            services.AddScoped<IPaymentService, PaymentService>();
            //Zalo
            services.AddScoped<IZaloPayService, ZaloPayService>();
            return services;
        }
    }
}
