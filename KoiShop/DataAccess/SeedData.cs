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
                    PasswordHash = "ck5KtRpnJNYKaItaIkV0qYhrZUYzMCAFkjyESYu3Hlc8xEx+Rj+9/HCZnkgOMuGoODqBt5gQCuIxINet7RRioA==",
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
                    PasswordHash = "ck5KtRpnJNYKaItaIkV0qYhrZUYzMCAFkjyESYu3Hlc8xEx+Rj+9/HCZnkgOMuGoODqBt5gQCuIxINet7RRioA==",
                    Phone = "0123456788",
                    RoleId = 2,
                    Status = UserStatusEnum.Active
                };
                context.Users.Add(admin);
                await context.SaveChangesAsync();
            }

            if (!context.Categories.Any())
            {
                context.Categories.AddRange(new List<Category>
    {
        new Category
        {
            Name = "Kohaku",
            
            Description = "Cá Koi Kohaku là một trong những loại phổ biến nhất, với nền trắng và các mảng đỏ.",
            ImageUrl = "https://drive.google.com/uc?export=view&id=1mNbQ4ysvXLkx6wmPqD1hvyY6Zi8gXxZc",
            OriginCountry = "Japan",
            
        },
        new Category
        {
            Name = "Taisho Sanshoku",
            
            Description = "Loại cá này có màu nền trắng với các mảng đỏ và đen.",
            ImageUrl = "url_to_image_of_taisho_sanshoku",
            OriginCountry = "Japan",
           
        },
        new Category
        {
            Name = "Shiro Utsuri",
            
            Description = "Cá Koi Shiro Utsuri có nền trắng với các đốm đen.",
            ImageUrl = "url_to_image_of_shiro_utsuri",
            OriginCountry = "Japan",
            
        },
        new Category
        {
            Name = "Showa Sanshoku",
            
            Description = "Loại cá này có màu nền đen với các mảng đỏ và trắng.",
            ImageUrl = "url_to_image_of_showa_sanshoku",
            OriginCountry = "Japan",
            
        },
        new Category
        {
            Name = "Goshiki",
            
            Description = "Cá Koi Goshiki có nền xanh hoặc trắng với các mảng đỏ.",
            ImageUrl = "url_to_image_of_goshiki",
            OriginCountry = "Japan",
            
        },
        new Category
        {
            Name = "Aka Matsuba",
            
            Description = "Cá Koi Aka Matsuba có nền đỏ với các đốm vàng.",
            ImageUrl = "url_to_image_of_aka_matsuba",
            OriginCountry = "Japan",
            
        }
        //+++
    });

                await context.SaveChangesAsync();
            }

        }
    }

    public class EmailSettings
    {
        public string SmtpServer { get; set; }
        public int SmtpPort { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FromEmail { get; set; }
    }

}
