using Microsoft.EntityFrameworkCore;
using RestaurantManagementSystem.Areas.Admin.Models;
using RestaurantManagementSystem.Areas.Customer.Models;
using RestaurantManagementSystem.Areas.Manager.Models;
using RestaurantManagementSystem.Areas.StockManager.Models;
using RestaurantManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantManagementSystem.Database
{
    public class DatabaseContext:DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext>options):base(options)
        {


        }

        public DbSet<Customers> Customers { get; set; }
        public DbSet<FoodItem> FoodItems { get; set; }
        public DbSet<Ingredient> Ingredient { get; set; }
        public DbSet<MealHour> MealHour { get; set; }
        public DbSet<Offer> Offer { get; set; }         
        public DbSet<RequiredMaterial> RequiredMaterial { get; set; }         
        public DbSet<Table> Table { get; set; }         
        public DbSet<StockDetails> stockDetails { get; set; }   
        public DbSet<CustomerOrderDetails> CustomerOrderDetails { get; set; }   
        public DbSet<CustomerOrderedTable> CustomerOrderedTable { get; set; }   
            
    }
}
