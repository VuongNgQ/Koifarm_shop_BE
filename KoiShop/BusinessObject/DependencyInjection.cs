using BusinessObject.IService;
using BusinessObject.Service;
using DataAccess;
using DataAccess.IRepo;
using DataAccess.Repo;
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
            //Role
            services.AddScoped<IRoleRepo, RoleRepo>();
            services.AddScoped<IRoleService, RoleService>();
            //Fish Status
            services.AddScoped<IProductStatusRepo, ProductStatusRepo>();
            services.AddScoped<IProductStatusService, ProductStatusService>();
            //Fish Package
            services.AddScoped<IFishPackageService, FishPackageService>();
            services.AddScoped<IFishPackageRepo, FishPackageRepo>();
            return services;
        }
    }
}
