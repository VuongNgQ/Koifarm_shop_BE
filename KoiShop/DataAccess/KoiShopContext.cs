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
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<UserCart> UserCarts { get; set; }
        public DbSet<FishSingle> Fish { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductStatus> ProductStatuses { get; set; }
        public DbSet<FishPackage> FishPackages { get; set; }
        public DbSet<FishConsignment> FishConsignments { get; set; }
        public DbSet<PackageConsignment> PackageConsignments { get; set; }
        public DbSet<ConsignmentType> ConsignmentTypes { get; set; }
        public DbSet<ConsignmentStatus> ConsignmentStatuses { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }
        public DbSet<FAQ> FAQs { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<UserAddress> UserAddresses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // RolePermission (many-to-many relationship)
            modelBuilder.Entity<RolePermission>()
                .HasKey(rp => new { rp.RoleId, rp.PermissionId });
            modelBuilder.Entity<RolePermission>()
                .HasOne(rp => rp.Role)
                .WithMany(r => r.RolePermissions)
                .HasForeignKey(rp => rp.RoleId);
            modelBuilder.Entity<RolePermission>()
                .HasOne(rp => rp.Permission)
                .WithMany(p => p.RolePermissions)
                .HasForeignKey(rp => rp.PermissionId);

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

            // PaymentMethod and Order (one-to-many)
            modelBuilder.Entity<Order>()
                .HasOne(o => o.PaymentMethod)
                .WithMany(pm => pm.Orders)
                .HasForeignKey(o => o.PaymentMethodId);

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
            modelBuilder.Entity<FishSingle>()
                .HasOne(f => f.Category)
                .WithMany(c => c.Fish)
                .HasForeignKey(f => f.CategoryId);

            // Fish and ProductStatus (one-to-many)
            modelBuilder.Entity<FishSingle>()
                .HasOne(f => f.Status)
                .WithMany(ps => ps.Fish)
                .HasForeignKey(f => f.StatusId);
            // Fish Package and ProductStatus (one-to-many)
            modelBuilder.Entity<FishPackage>()
                .HasOne(f => f.Status)
                .WithMany(ps => ps.FishPackages)
                .HasForeignKey(f => f.StatusId);

            // FishConsignment and Fish (one-to-many)
            modelBuilder.Entity<FishConsignment>()
                .HasOne(fc => fc.Fish)
                .WithMany(f => f.Consignments)
                .HasForeignKey(fc => fc.FishId);

            // FishConsignment and User (one-to-many)
            modelBuilder.Entity<FishConsignment>()
                .HasOne(fc => fc.User)
                .WithMany(u => u.FishConsignments)
                .HasForeignKey(fc => fc.UserId);

            // FishConsignment and ConsignmentType (one-to-many)
            modelBuilder.Entity<FishConsignment>()
                .HasOne(fc => fc.ConsignmentType)
                .WithMany(ct => ct.FishConsignments)
                .HasForeignKey(fc => fc.ConsignmentTypeId);

            // FishConsignment and ConsignmentStatus (one-to-many)
            modelBuilder.Entity<FishConsignment>()
                .HasOne(fc => fc.ConsignmentStatus)
                .WithMany(cs => cs.FishConsignments)
                .HasForeignKey(fc => fc.ConsignmentStatusId);

            // PackageConsignment and FishPackage (one-to-many)
            modelBuilder.Entity<PackageConsignment>()
                .HasOne(pc => pc.Package)
                .WithMany(fp => fp.Consignments)
                .HasForeignKey(pc => pc.PackageId);

            // PackageConsignment and User (one-to-many)
            modelBuilder.Entity<PackageConsignment>()
                .HasOne(pc => pc.User)
                .WithMany(u => u.PackageConsignments)
                .HasForeignKey(pc => pc.UserId);

            // PackageConsignment and ConsignmentType (one-to-many)
            modelBuilder.Entity<PackageConsignment>()
                .HasOne(pc => pc.ConsignmentType)
                .WithMany(ct => ct.PackageConsignments)
                .HasForeignKey(pc => pc.ConsignmentTypeId);

            // PackageConsignment and ConsignmentStatus (one-to-many)
            modelBuilder.Entity<PackageConsignment>()
                .HasOne(pc => pc.ConsignmentStatus)
                .WithMany(cs => cs.PackageConsignments)
                .HasForeignKey(pc => pc.ConsignmentStatusId);

            // Feedback and User (one-to-many)
            modelBuilder.Entity<Feedback>()
                .HasOne(fb => fb.User)
                .WithMany(u => u.Feedbacks)
                .HasForeignKey(fb => fb.UserId);

            // Feedback and Fish (one-to-many)
            modelBuilder.Entity<Feedback>()
                .HasOne(fb => fb.Fish)
                .WithMany(f => f.Feedbacks)
                .HasForeignKey(fb => fb.FishId);

            // Feedback and Package (one-to-many)
            modelBuilder.Entity<Feedback>()
                .HasOne(fb => fb.Package)
                .WithMany(f => f.Feedbacks)
                .HasForeignKey(fb => fb.FishId);

            // Order and OrderStatus (one-to-many)
            modelBuilder.Entity<Order>()
                .HasOne(o => o.OrderStatus)
                .WithMany(os => os.Orders)
                .HasForeignKey(o => o.StatusId);

            // Address and Order (one-to-many)
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Address)
                .WithMany(os => os.Orders)
                .HasForeignKey(o => o.AddressId);

            base.OnModelCreating(modelBuilder);
        }

    }
}
