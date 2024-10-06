﻿// <auto-generated />
using System;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DataAccess.Migrations
{
    [DbContext(typeof(KoiShopContext))]
    [Migration("20241004072117_2.1")]
    partial class _21
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DataAccess.Entity.Address", b =>
                {
                    b.Property<int>("AddressId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AddressId"));

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("District")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Street")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("AddressId");

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("DataAccess.Entity.Blog", b =>
                {
                    b.Property<int>("BlogId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BlogId"));

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("BlogId");

                    b.HasIndex("UserId");

                    b.ToTable("Blogs");
                });

            modelBuilder.Entity("DataAccess.Entity.CartItem", b =>
                {
                    b.Property<int>("CartItemId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CartItemId"));

                    b.Property<int?>("FishId")
                        .HasColumnType("int");

                    b.Property<int?>("PackageId")
                        .HasColumnType("int");

                    b.Property<int?>("Quantity")
                        .HasColumnType("int");

                    b.Property<int?>("UserCartId")
                        .HasColumnType("int");

                    b.HasKey("CartItemId");

                    b.HasIndex("FishId");

                    b.HasIndex("PackageId");

                    b.HasIndex("UserCartId");

                    b.ToTable("CartItems");
                });

            modelBuilder.Entity("DataAccess.Entity.Category", b =>
                {
                    b.Property<int>("CategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CategoryId"));

                    b.Property<string>("CategoryStatus")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OriginCountry")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal?>("Size")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("CategoryId");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("DataAccess.Entity.ConsignmentStatus", b =>
                {
                    b.Property<int>("ConsignmentStatusId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ConsignmentStatusId"));

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ConsignmentStatusId");

                    b.ToTable("ConsignmentStatuses");
                });

            modelBuilder.Entity("DataAccess.Entity.ConsignmentType", b =>
                {
                    b.Property<int>("ConsignmentTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ConsignmentTypeId"));

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ConsignmentTypeId");

                    b.ToTable("ConsignmentTypes");
                });

            modelBuilder.Entity("DataAccess.Entity.FAQ", b =>
                {
                    b.Property<int>("FaqId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("FaqId"));

                    b.Property<string>("Answer")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Question")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("FaqId");

                    b.ToTable("FAQs");
                });

            modelBuilder.Entity("DataAccess.Entity.Feedback", b =>
                {
                    b.Property<int>("FeedbackId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("FeedbackId"));

                    b.Property<DateTime?>("AddedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Comment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("FishId")
                        .HasColumnType("int");

                    b.Property<int?>("Rating")
                        .HasColumnType("int");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("FeedbackId");

                    b.HasIndex("FishId");

                    b.HasIndex("UserId");

                    b.ToTable("Feedbacks");
                });

            modelBuilder.Entity("DataAccess.Entity.FishConsignment", b =>
                {
                    b.Property<int>("FishConsignmentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("FishConsignmentId"));

                    b.Property<int?>("ConsignmentStatusId")
                        .HasColumnType("int");

                    b.Property<int?>("ConsignmentTypeId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("FishId")
                        .HasColumnType("int");

                    b.Property<int?>("FishStatusId")
                        .HasColumnType("int");

                    b.Property<decimal?>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime?>("ReceiveDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("TransferDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("FishConsignmentId");

                    b.HasIndex("ConsignmentStatusId");

                    b.HasIndex("ConsignmentTypeId");

                    b.HasIndex("FishId");

                    b.HasIndex("UserId");

                    b.ToTable("FishConsignments");
                });

            modelBuilder.Entity("DataAccess.Entity.FishPackage", b =>
                {
                    b.Property<int>("FishPackageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("FishPackageId"));

                    b.Property<int?>("Age")
                        .HasColumnType("int");

                    b.Property<int?>("CategoryId")
                        .HasColumnType("int");

                    b.Property<decimal?>("DailyFood")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Gender")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("NumberOfFish")
                        .HasColumnType("int");

                    b.Property<int?>("RemainingPackage")
                        .HasColumnType("int");

                    b.Property<decimal?>("Size")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int?>("StatusId")
                        .HasColumnType("int");

                    b.Property<decimal?>("TotalPrice")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("FishPackageId");

                    b.HasIndex("CategoryId");

                    b.HasIndex("StatusId");

                    b.ToTable("FishPackages");
                });

            modelBuilder.Entity("DataAccess.Entity.FishSingle", b =>
                {
                    b.Property<int>("FishSingleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("FishSingleId"));

                    b.Property<int?>("Age")
                        .HasColumnType("int");

                    b.Property<int?>("CategoryId")
                        .HasColumnType("int");

                    b.Property<decimal?>("DailyFood")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Gender")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal?>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int?>("QuantityInStock")
                        .HasColumnType("int");

                    b.Property<decimal?>("Size")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int?>("StatusId")
                        .HasColumnType("int");

                    b.HasKey("FishSingleId");

                    b.HasIndex("CategoryId");

                    b.HasIndex("StatusId");

                    b.ToTable("Fish");
                });

            modelBuilder.Entity("DataAccess.Entity.Order", b =>
                {
                    b.Property<int>("OrderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OrderId"));

                    b.Property<int?>("AddressId")
                        .HasColumnType("int");

                    b.Property<bool?>("IsSent")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("OrderDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("PaymentMethodId")
                        .HasColumnType("int");

                    b.Property<int?>("StatusId")
                        .HasColumnType("int");

                    b.Property<decimal?>("TotalAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("TotalPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("OrderId");

                    b.HasIndex("AddressId");

                    b.HasIndex("PaymentMethodId");

                    b.HasIndex("StatusId");

                    b.HasIndex("UserId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("DataAccess.Entity.OrderItem", b =>
                {
                    b.Property<int>("OrderItemId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OrderItemId"));

                    b.Property<int?>("FishId")
                        .HasColumnType("int");

                    b.Property<int?>("OrderId")
                        .HasColumnType("int");

                    b.Property<int?>("PackageId")
                        .HasColumnType("int");

                    b.Property<decimal?>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int?>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("OrderItemId");

                    b.HasIndex("FishId");

                    b.HasIndex("OrderId");

                    b.HasIndex("PackageId");

                    b.ToTable("OrderItems");
                });

            modelBuilder.Entity("DataAccess.Entity.OrderStatus", b =>
                {
                    b.Property<int>("OrderStatusId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OrderStatusId"));

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("OrderStatusId");

                    b.ToTable("OrderStatuses");
                });

            modelBuilder.Entity("DataAccess.Entity.PackageConsignment", b =>
                {
                    b.Property<int>("PackageConsignmentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PackageConsignmentId"));

                    b.Property<int?>("ConsignmentStatusId")
                        .HasColumnType("int");

                    b.Property<int?>("ConsignmentTypeId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("PackageId")
                        .HasColumnType("int");

                    b.Property<int?>("PackageStatusId")
                        .HasColumnType("int");

                    b.Property<decimal?>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime?>("ReceiveDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("TransferDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("PackageConsignmentId");

                    b.HasIndex("ConsignmentStatusId");

                    b.HasIndex("ConsignmentTypeId");

                    b.HasIndex("PackageId");

                    b.HasIndex("UserId");

                    b.ToTable("PackageConsignments");
                });

            modelBuilder.Entity("DataAccess.Entity.PaymentMethod", b =>
                {
                    b.Property<int>("PaymentMethodId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PaymentMethodId"));

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PaymentMethodId");

                    b.ToTable("PaymentMethods");
                });

            modelBuilder.Entity("DataAccess.Entity.Permission", b =>
                {
                    b.Property<int>("PermissionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PermissionId"));

                    b.Property<string>("PermissionName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PermissionId");

                    b.ToTable("Permissions");
                });

            modelBuilder.Entity("DataAccess.Entity.ProductStatus", b =>
                {
                    b.Property<int>("ProductStatusId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProductStatusId"));

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ProductStatusId");

                    b.ToTable("ProductStatuses");
                });

            modelBuilder.Entity("DataAccess.Entity.Role", b =>
                {
                    b.Property<int>("RoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RoleId"));

                    b.Property<string>("RoleName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("RoleId");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("DataAccess.Entity.RolePermission", b =>
                {
                    b.Property<int?>("RoleId")
                        .HasColumnType("int");

                    b.Property<int?>("PermissionId")
                        .HasColumnType("int");

                    b.HasKey("RoleId", "PermissionId");

                    b.HasIndex("PermissionId");

                    b.ToTable("RolePermissions");
                });

            modelBuilder.Entity("DataAccess.Entity.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"));

                    b.Property<DateTime?>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("RoleId")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.HasIndex("RoleId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("DataAccess.Entity.UserAddress", b =>
                {
                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.Property<int?>("AddressId")
                        .HasColumnType("int");

                    b.HasKey("UserId", "AddressId");

                    b.HasIndex("AddressId");

                    b.ToTable("UserAddresses");
                });

            modelBuilder.Entity("DataAccess.Entity.UserCart", b =>
                {
                    b.Property<int>("UserCartId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserCartId"));

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("UserCartId");

                    b.HasIndex("UserId");

                    b.ToTable("UserCarts");
                });

            modelBuilder.Entity("DataAccess.Entity.Blog", b =>
                {
                    b.HasOne("DataAccess.Entity.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("DataAccess.Entity.CartItem", b =>
                {
                    b.HasOne("DataAccess.Entity.FishSingle", "Fish")
                        .WithMany("CartItems")
                        .HasForeignKey("FishId");

                    b.HasOne("DataAccess.Entity.FishPackage", "Package")
                        .WithMany("CartItems")
                        .HasForeignKey("PackageId");

                    b.HasOne("DataAccess.Entity.UserCart", "UserCart")
                        .WithMany("CartItems")
                        .HasForeignKey("UserCartId");

                    b.Navigation("Fish");

                    b.Navigation("Package");

                    b.Navigation("UserCart");
                });

            modelBuilder.Entity("DataAccess.Entity.Feedback", b =>
                {
                    b.HasOne("DataAccess.Entity.FishPackage", "Package")
                        .WithMany("Feedbacks")
                        .HasForeignKey("FishId");

                    b.HasOne("DataAccess.Entity.FishSingle", "Fish")
                        .WithMany("Feedbacks")
                        .HasForeignKey("FishId");

                    b.HasOne("DataAccess.Entity.User", "User")
                        .WithMany("Feedbacks")
                        .HasForeignKey("UserId");

                    b.Navigation("Fish");

                    b.Navigation("Package");

                    b.Navigation("User");
                });

            modelBuilder.Entity("DataAccess.Entity.FishConsignment", b =>
                {
                    b.HasOne("DataAccess.Entity.ConsignmentStatus", "ConsignmentStatus")
                        .WithMany("FishConsignments")
                        .HasForeignKey("ConsignmentStatusId");

                    b.HasOne("DataAccess.Entity.ConsignmentType", "ConsignmentType")
                        .WithMany("FishConsignments")
                        .HasForeignKey("ConsignmentTypeId");

                    b.HasOne("DataAccess.Entity.FishSingle", "Fish")
                        .WithMany("Consignments")
                        .HasForeignKey("FishId");

                    b.HasOne("DataAccess.Entity.User", "User")
                        .WithMany("FishConsignments")
                        .HasForeignKey("UserId");

                    b.Navigation("ConsignmentStatus");

                    b.Navigation("ConsignmentType");

                    b.Navigation("Fish");

                    b.Navigation("User");
                });

            modelBuilder.Entity("DataAccess.Entity.FishPackage", b =>
                {
                    b.HasOne("DataAccess.Entity.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId");

                    b.HasOne("DataAccess.Entity.ProductStatus", "Status")
                        .WithMany("FishPackages")
                        .HasForeignKey("StatusId");

                    b.Navigation("Category");

                    b.Navigation("Status");
                });

            modelBuilder.Entity("DataAccess.Entity.FishSingle", b =>
                {
                    b.HasOne("DataAccess.Entity.Category", "Category")
                        .WithMany("Fish")
                        .HasForeignKey("CategoryId");

                    b.HasOne("DataAccess.Entity.ProductStatus", "Status")
                        .WithMany("Fish")
                        .HasForeignKey("StatusId");

                    b.Navigation("Category");

                    b.Navigation("Status");
                });

            modelBuilder.Entity("DataAccess.Entity.Order", b =>
                {
                    b.HasOne("DataAccess.Entity.Address", "Address")
                        .WithMany("Orders")
                        .HasForeignKey("AddressId");

                    b.HasOne("DataAccess.Entity.PaymentMethod", "PaymentMethod")
                        .WithMany("Orders")
                        .HasForeignKey("PaymentMethodId");

                    b.HasOne("DataAccess.Entity.OrderStatus", "OrderStatus")
                        .WithMany("Orders")
                        .HasForeignKey("StatusId");

                    b.HasOne("DataAccess.Entity.User", "User")
                        .WithMany("Orders")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("Address");

                    b.Navigation("OrderStatus");

                    b.Navigation("PaymentMethod");

                    b.Navigation("User");
                });

            modelBuilder.Entity("DataAccess.Entity.OrderItem", b =>
                {
                    b.HasOne("DataAccess.Entity.FishSingle", "Fish")
                        .WithMany("OrderItems")
                        .HasForeignKey("FishId");

                    b.HasOne("DataAccess.Entity.Order", "Order")
                        .WithMany("OrderItems")
                        .HasForeignKey("OrderId");

                    b.HasOne("DataAccess.Entity.FishPackage", "Package")
                        .WithMany("OrderItems")
                        .HasForeignKey("PackageId");

                    b.Navigation("Fish");

                    b.Navigation("Order");

                    b.Navigation("Package");
                });

            modelBuilder.Entity("DataAccess.Entity.PackageConsignment", b =>
                {
                    b.HasOne("DataAccess.Entity.ConsignmentStatus", "ConsignmentStatus")
                        .WithMany("PackageConsignments")
                        .HasForeignKey("ConsignmentStatusId");

                    b.HasOne("DataAccess.Entity.ConsignmentType", "ConsignmentType")
                        .WithMany("PackageConsignments")
                        .HasForeignKey("ConsignmentTypeId");

                    b.HasOne("DataAccess.Entity.FishPackage", "Package")
                        .WithMany("Consignments")
                        .HasForeignKey("PackageId");

                    b.HasOne("DataAccess.Entity.User", "User")
                        .WithMany("PackageConsignments")
                        .HasForeignKey("UserId");

                    b.Navigation("ConsignmentStatus");

                    b.Navigation("ConsignmentType");

                    b.Navigation("Package");

                    b.Navigation("User");
                });

            modelBuilder.Entity("DataAccess.Entity.RolePermission", b =>
                {
                    b.HasOne("DataAccess.Entity.Permission", "Permission")
                        .WithMany("RolePermissions")
                        .HasForeignKey("PermissionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DataAccess.Entity.Role", "Role")
                        .WithMany("RolePermissions")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Permission");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("DataAccess.Entity.User", b =>
                {
                    b.HasOne("DataAccess.Entity.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("DataAccess.Entity.UserAddress", b =>
                {
                    b.HasOne("DataAccess.Entity.Address", "Address")
                        .WithMany("UserAddresses")
                        .HasForeignKey("AddressId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("DataAccess.Entity.User", "User")
                        .WithMany("UserAddresses")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Address");

                    b.Navigation("User");
                });

            modelBuilder.Entity("DataAccess.Entity.UserCart", b =>
                {
                    b.HasOne("DataAccess.Entity.User", "User")
                        .WithMany("UserCarts")
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("DataAccess.Entity.Address", b =>
                {
                    b.Navigation("Orders");

                    b.Navigation("UserAddresses");
                });

            modelBuilder.Entity("DataAccess.Entity.Category", b =>
                {
                    b.Navigation("Fish");
                });

            modelBuilder.Entity("DataAccess.Entity.ConsignmentStatus", b =>
                {
                    b.Navigation("FishConsignments");

                    b.Navigation("PackageConsignments");
                });

            modelBuilder.Entity("DataAccess.Entity.ConsignmentType", b =>
                {
                    b.Navigation("FishConsignments");

                    b.Navigation("PackageConsignments");
                });

            modelBuilder.Entity("DataAccess.Entity.FishPackage", b =>
                {
                    b.Navigation("CartItems");

                    b.Navigation("Consignments");

                    b.Navigation("Feedbacks");

                    b.Navigation("OrderItems");
                });

            modelBuilder.Entity("DataAccess.Entity.FishSingle", b =>
                {
                    b.Navigation("CartItems");

                    b.Navigation("Consignments");

                    b.Navigation("Feedbacks");

                    b.Navigation("OrderItems");
                });

            modelBuilder.Entity("DataAccess.Entity.Order", b =>
                {
                    b.Navigation("OrderItems");
                });

            modelBuilder.Entity("DataAccess.Entity.OrderStatus", b =>
                {
                    b.Navigation("Orders");
                });

            modelBuilder.Entity("DataAccess.Entity.PaymentMethod", b =>
                {
                    b.Navigation("Orders");
                });

            modelBuilder.Entity("DataAccess.Entity.Permission", b =>
                {
                    b.Navigation("RolePermissions");
                });

            modelBuilder.Entity("DataAccess.Entity.ProductStatus", b =>
                {
                    b.Navigation("Fish");

                    b.Navigation("FishPackages");
                });

            modelBuilder.Entity("DataAccess.Entity.Role", b =>
                {
                    b.Navigation("RolePermissions");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("DataAccess.Entity.User", b =>
                {
                    b.Navigation("Feedbacks");

                    b.Navigation("FishConsignments");

                    b.Navigation("Orders");

                    b.Navigation("PackageConsignments");

                    b.Navigation("UserAddresses");

                    b.Navigation("UserCarts");
                });

            modelBuilder.Entity("DataAccess.Entity.UserCart", b =>
                {
                    b.Navigation("CartItems");
                });
#pragma warning restore 612, 618
        }
    }
}