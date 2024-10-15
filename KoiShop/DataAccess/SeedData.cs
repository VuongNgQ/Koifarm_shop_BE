using DataAccess.Entity;
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
                    Password = "password",
                    Phone = "0123456789",
                    RoleId = 1,
                    Status = "Active"
                };
                context.Users.Add(admin);
                await context.SaveChangesAsync();
            }

            // Thêm các dữ liệu khởi tạo khác nếu cần
        }
    }
}
