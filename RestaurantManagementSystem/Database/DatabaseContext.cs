using Microsoft.EntityFrameworkCore;
using RestaurantManagementSystem.Areas.Chef.Models;
using RestaurantManagementSystem.Areas.Customer.Models;
using RestaurantManagementSystem.Areas.Staff.Models;
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
        public DbSet<AttendanceRecord> AttendanceRecords { get; set; } 
        public DbSet<SMS> Smses { get; set; }  
        public DbSet<StockDetails> StockDetails { get; set; }   
        public DbSet<Order> Orders { get; set; }    
       
            
    }
}
