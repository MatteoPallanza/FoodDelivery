using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace FoodDelivery.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<UserAddress> UserAddresses { get; set; }

        public DbSet<UpgradeRequest> UpgradeRequests { get; set; }

        public DbSet<Restaurateur> Restaurateurs { get; set; } 
        
        public DbSet<RestaurateurCategory> RestaurateurCategories { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<ProductCategory> ProductCategories { get; set; }

        public DbSet<RiderFee> RiderFees { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(p => p.UserAddresses)
                .WithOne(p => p.User);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(p => p.UpgradeRequests)
                .WithOne(p => p.User);

            modelBuilder.Entity<ApplicationUser>()
                .HasOne(p => p.RestaurateurDetail)
                .WithOne(p => p.User);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(p => p.Products)
                .WithOne(p => p.User);

            modelBuilder.Entity<ProductCategory>()
                .HasMany(p => p.Products)
                .WithOne(p => p.Category);

            modelBuilder.Entity<RestaurateurCategory>()
                .HasMany(p => p.Restaurateurs)
                .WithOne(p => p.Category);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(p => p.UserOrders)
                .WithOne(p => p.User);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(p => p.RestaurateurOrders)
                .WithOne(p => p.Restaurateur);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(p => p.RiderOrders)
                .WithOne(p => p.Rider);

            modelBuilder.Entity<OrderDetail>().HasKey(c => new { c.OrderId, c.ProductId });
            modelBuilder.Entity<OrderDetail>().ToTable("OrderDetails");
        }
    }
}
