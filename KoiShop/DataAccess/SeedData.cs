using DataAccess.Entity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccess
{
    public static class SeedData
    {
        public static async Task SeedDataAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<KoiShopContext>();

            await context.Database.EnsureCreatedAsync();

            // Kiểm tra và thêm các Role
            if (!context.Roles.Any())
            {
                context.Roles.AddRange(new List<Role>
            {
                new Role { RoleName = "Admin"},
                new Role { RoleName = "Manager" },
                new Role { RoleName = "Staff" },
                new Role { RoleName = "Customer" }
            });

                await context.SaveChangesAsync();
            }

            if (!context.Users.Any(u => u.Email == "admin@admin.com"))
            {
                var admin = new User
                {
                    Name = "Admin",
                    Email = "admin@admin.com",
                    PasswordHash = "hTIB5M6ztDPGX1Ms1YGcl+I1FvznkP4oe7dRSEA7oIhizNEyhqwM6e5Dl1hiP29tLNh3mgtv/wDWmrIY3QNlRA==\r\n",
                    Phone = "0123456789",
                    RoleId = 1,
                    Status = UserStatusEnum.Active
                };
                context.Users.Add(admin);
                await context.SaveChangesAsync();
            }
            if (!context.Users.Any(u => u.Email == "manager@manager.com"))
            {
                var admin = new User
                {
                    Name = "Manager",
                    Email = "manager@manager.com",
                    PasswordHash = "hTIB5M6ztDPGX1Ms1YGcl+I1FvznkP4oe7dRSEA7oIhizNEyhqwM6e5Dl1hiP29tLNh3mgtv/wDWmrIY3QNlRA==\r\n",
                    Phone = "0123456788",
                    RoleId = 2,
                    Status = UserStatusEnum.Active
                };
                context.Users.Add(admin);
                await context.SaveChangesAsync();
            }

            // Thêm các dữ liệu khởi tạo khác nếu cần
        }
    }
}
