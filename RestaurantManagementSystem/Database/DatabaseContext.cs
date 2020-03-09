
﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;



using Microsoft.EntityFrameworkCore;
using RestaurantManagementSystem.Areas.Admin.Models;
using RestaurantManagementSystem.Areas.Customer.Models;
using RestaurantManagementSystem.Areas.Manager.Models;
using RestaurantManagementSystem.Areas.StockManager.Models;


namespace RestaurantManagementSystem.Database
{
    public class DatabaseContext :IdentityDbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext>options)
            :base(options){}
        public DbSet<Customers> Customers { get; set; }
        public DbSet<FoodItem> FoodItems { get; set; }
        public DbSet<Ingredient> Ingredient { get; set; }
        public DbSet<MealHour> MealHour { get; set; }
        public DbSet<Offer> Offer { get; set; }         
        public DbSet<RequiredMaterial> RequiredMaterial { get; set; }         
        public DbSet<Table> Table { get; set; }         
        public DbSet<StockDetails> StockDetails { get; set; }        
        public DbSet<CustomerOrderDetails> CustomerOrderDetails { get; set; }   
        public DbSet<CustomerOrderedTable> CustomerOrderedTable { get; set; }
       
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // any guid
            const string ADMIN_ID = "a18be9c0-aa65-4af8-bd17-00bd9344e575";
            // any guid, but nothing is against to use the same one
            const string ROLE_ID = ADMIN_ID;
            builder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Id = ROLE_ID,
                Name = "admin",
                NormalizedName = "ADMIN",
                
            });

            var hasher = new PasswordHasher<IdentityUser>();
            builder.Entity<IdentityUser>().HasData(new IdentityUser
            {
                Id = ADMIN_ID,
                UserName = "admin",
                NormalizedUserName = "admin",
                Email = "admin@gmail.com",
                NormalizedEmail = "ADMIN@gmail.com",
                EmailConfirmed = false,
                PasswordHash = hasher.HashPassword(null, "admin"),
                AccessFailedCount=0
            });

            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = ROLE_ID,
                UserId = ADMIN_ID
            });
        }
    }
}
