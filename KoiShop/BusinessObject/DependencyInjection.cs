using BusinessObject.IService;
using BusinessObject.Service;
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
        public static IServiceCollection AddService(this IServiceCollection services, string DatabaseConnection)
        {
            services.AddScoped<IUserService, UserService>();
            return services;
        }
    }
}
