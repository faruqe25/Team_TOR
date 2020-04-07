
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RestaurantManagementSystem.Areas.Admin.Models;
using RestaurantManagementSystem.Areas.Customer.Models;
using RestaurantManagementSystem.Areas.Manager.Models;
using RestaurantManagementSystem.Areas.StockManager.Models;


namespace RestaurantManagementSystem.Database
{
    public class DatabaseContext : IdentityDbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options) { }
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
            const string ADMIN_ID = "a18be9c0-ax65-4af8-bd17-00bd9344e575";
            const string Customer_ID = "arttS5-ab65-4sfpx-bd17-00bd8744e575";
            const string Manager_ID = "darva-aa68-4af8-bd17-aggfdsgfgrtgg";
            const string StockManager_ID = "8544ra-aa75-4af8-bd17-00rwrd9344e575";
            // any guid, but nothing is against to use the same one
            const string Admin_ROLE_ID = ADMIN_ID;
            const string Manager_ROLE_ID = Manager_ID;
            const string Customer_ROLE_ID = Customer_ID;
            const string StockManager_ROLE_ID = StockManager_ID;
            var hasher = new PasswordHasher<IdentityUser>();
            builder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Id = Admin_ROLE_ID,
                Name = "Admin",
                NormalizedName = "ADMIN",

            });

            builder.Entity<IdentityUser>().HasData(new IdentityUser
            {
                Id = ADMIN_ID,
                UserName = "admin@gmail.com",
                NormalizedUserName = "admin",
                Email = "admin@gmail.com",
                NormalizedEmail = "ADMIN@gmail.com",
                EmailConfirmed = false,
                PasswordHash = hasher.HashPassword(null, "admin"),
                AccessFailedCount = 0
            });
            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = Admin_ROLE_ID,
                UserId = ADMIN_ID
            });
            builder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Id = Customer_ROLE_ID,
                Name = "Customer",
                NormalizedName = "CUSTOMER",

            });
            builder.Entity<IdentityUser>().HasData(new IdentityUser
            {
                Id = Customer_ID,
                UserName = "customer@gmail.com",
                NormalizedUserName = "customer",
                Email = "customer@gmail.com",
                NormalizedEmail = "CUSTOMER@gmail.com",
                EmailConfirmed = false,
                PasswordHash = hasher.HashPassword(null, "customer"),
                AccessFailedCount = 0,
                PhoneNumber = "015867158",
            });

            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = Customer_ROLE_ID,
                UserId = Customer_ID
            });
            builder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Id = Manager_ROLE_ID,
                Name = "Manager",
                NormalizedName = "MANAGER",

            });
            builder.Entity<IdentityUser>().HasData(new IdentityUser
            {
                Id = Manager_ID,
                UserName = "manager@gmail.com",
                NormalizedUserName = "manager",
                Email = "manager@gmail.com",
                NormalizedEmail = "MANAGER@gmail.com",
                EmailConfirmed = false,
                PasswordHash = hasher.HashPassword(null, "manager"),
                AccessFailedCount = 0,
               
            });
            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = Manager_ROLE_ID,
                UserId = Manager_ID
            });
            builder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Id = StockManager_ROLE_ID,
                Name = "StockManager",
                NormalizedName = "STOCKMANAGER",

            });
            builder.Entity<IdentityUser>().HasData(new IdentityUser
            {
                Id = StockManager_ID,
                UserName = "stockmanager@gmail.com",
                NormalizedUserName = "StockManager",
                Email = "stockmanager@gmail.com",
                NormalizedEmail = "STOCKMANAGER@gmail.com",
                EmailConfirmed = false,
                PasswordHash = hasher.HashPassword(null, "stockmanager"),
                AccessFailedCount = 0
            });
            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = StockManager_ROLE_ID,
                UserId = StockManager_ID
            });
            builder.Entity<Table>().HasData(new Table
            {
                TableId = 1,
                TableCapacity = 0,
                TableNumber = "Special Table",
                BookedStatus = true,
                BookingPrice = 0
            });
            builder.Entity<Customers>().HasData(new Customers
            {
                CustomersId = 1,
                CustomersName = "Customer",
                MobileNumber = "015867158",
              
                
            });
        }
    }
}