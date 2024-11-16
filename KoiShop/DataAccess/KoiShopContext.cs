using DataAccess.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class KoiShopContext:DbContext
    {
        public KoiShopContext(DbContextOptions<KoiShopContext> options)
        : base(options)
        {

        }

        // DbSets for each entity
        public DbSet<User> Users { get; set; }
        public DbSet<PasswordResetToken> PasswordResetTokens { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Address> Addresses { get; set; }
        
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<UserCart> UserCarts { get; set; }
        public DbSet<Fish> Fish { get; set; }
        public DbSet<Category> Categories { get; set; }
        
        public DbSet<FishPackage> FishPackages { get; set; }
        public DbSet<FishConsignment> FishConsignments { get; set; }
        
        
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<SubImage> SubImages { get; set; }
        
        public DbSet<UserAddress> UserAddresses { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<UserFishOwnership> userFishOwnerships { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // UserAddress (many-to-many relationship)
            modelBuilder.Entity<UserAddress>()
                .HasKey(ua => new { ua.UserId, ua.AddressId });
            modelBuilder.Entity<UserAddress>()
                .HasOne(ua => ua.User)
                .WithMany(u => u.UserAddresses)
                .HasForeignKey(ua => ua.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<UserAddress>()
                .HasOne(ua => ua.Address)
                .WithMany(u=>u.UserAddresses)
                .HasForeignKey(ua => ua.AddressId)
                .OnDelete(DeleteBehavior.Restrict);
            //CategoryPackage(many-to-many)
            modelBuilder.Entity<CategoryPackage>()
                .HasKey(ua => new { ua.FishPackageId, ua.CategoryId });
            modelBuilder.Entity<CategoryPackage>()
                .HasOne(ua => ua.Package)
                .WithMany(u => u.CategoryPackages)
                .HasForeignKey(ua => ua.FishPackageId);
            modelBuilder.Entity<CategoryPackage>()
                .HasOne(ua => ua.Category)
                .WithMany(u => u.CategoryPackages)
                .HasForeignKey(ua => ua.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
            // User and Role (one-to-many)
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId);

            // User and Order (one-to-many)
            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            // OrderItem and Order (one-to-many)
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId);

            // OrderItem and Fish (one-to-many)
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Fish)
                .WithMany(f => f.OrderItems)
                .HasForeignKey(oi => oi.FishId);

            // OrderItem and FishPackage (one-to-many)
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Package)
                .WithMany(fp => fp.OrderItems)
                .HasForeignKey(oi => oi.PackageId);

            // CartItem and UserCart (one-to-many)
            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.UserCart)
                .WithMany(uc => uc.CartItems)
                .HasForeignKey(ci => ci.UserCartId);

            // OrderItem and Fish (one-to-many)
            modelBuilder.Entity<OrderItem>()
                .HasOne(ci => ci.Fish)
                .WithMany(f => f.OrderItems)
                .HasForeignKey(ci => ci.FishId);

            // CartItem and Fish (one-to-many)
            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Fish)
                .WithMany(f => f.CartItems)
                .HasForeignKey(ci => ci.FishId);

            // CartItem and FishPackage (one-to-many)
            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Package)
                .WithMany(fp => fp.CartItems)
                .HasForeignKey(ci => ci.PackageId);

            // CartItem and FishPackage (one-to-many)
            modelBuilder.Entity<OrderItem>()
                .HasOne(ci => ci.Package)
                .WithMany(fp => fp.OrderItems)
                .HasForeignKey(ci => ci.PackageId);

            // Fish and Category (one-to-many)
            modelBuilder.Entity<Fish>()
                .HasOne(f => f.Category)
                .WithMany(c => c.Fish)
                .HasForeignKey(f => f.CategoryId);
            // FishConsignment and Fish (one-to-many)
            modelBuilder.Entity<FishConsignment>()
                .HasOne(fc => fc.Fish)
                .WithMany(f => f.Consignments)
                .HasForeignKey(fc => fc.FishId)
                .OnDelete(DeleteBehavior.Restrict);
            // FishConsignment and User (one-to-many)
            modelBuilder.Entity<FishConsignment>()
                .HasOne(fc => fc.User)
                .WithMany(u => u.FishConsignments)
                .HasForeignKey(fc => fc.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Order)
                .WithMany(o => o.Payments)
                .HasForeignKey(p => p.OrderId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Payment>()
                .HasOne(p => p.FishConsignment)
                .WithMany(fc => fc.Payments)
                .HasForeignKey(p => p.FishConsignmentId)
                .OnDelete(DeleteBehavior.Restrict);

            //modelBuilder.Entity<FishConsignment>()
            //    .HasMany(fc => fc.Payments)
            //    .WithOne()
            //    .HasForeignKey(p => p.RelatedId)
            //    .OnDelete(DeleteBehavior.Cascade);
            // Configure User relationship with Payment
            //modelBuilder.Entity<Payment>()
            //    .HasOne(p => p.User)
            //    .WithMany()
            //    .HasForeignKey(p => p.UserId)
            //    .OnDelete(DeleteBehavior.Restrict);
            // Feedback and User (one-to-many)
            modelBuilder.Entity<Feedback>()
                .HasOne(fb => fb.User)
                .WithMany(u => u.Feedbacks)
                .HasForeignKey(fb => fb.UserId);
            // Feedback and Package (one-to-many)
            modelBuilder.Entity<Feedback>()
                .HasOne(fb => fb.Order)
                .WithMany(f => f.Feedbacks)
                .HasForeignKey(fb => fb.OrderId);
            // Address and Order (one-to-many)
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Address)
                .WithMany(os => os.Orders)
                .HasForeignKey(o => o.AddressId);
            // User and PasswordResetToken
            modelBuilder.Entity<PasswordResetToken>()
                .HasOne(p => p.User)
                .WithMany(u => u.PasswordResetTokens)
                .HasForeignKey(p => p.UserId);
            // User and OwnerShip (one-to-many)
            modelBuilder.Entity<UserFishOwnership>()
                .HasOne(o => o.User)
                .WithMany(u => u.UserFishOwnerships)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.NoAction);
            // Fish and OwnerShip (one-to-many)
            modelBuilder.Entity<UserFishOwnership>()
                .HasOne(o => o.Fish)
                .WithMany(u => u.UserFishOwnerships)
                .HasForeignKey(o => o.FishId)
                .OnDelete(DeleteBehavior.NoAction);
            base.OnModelCreating(modelBuilder);
        }

    }
}
