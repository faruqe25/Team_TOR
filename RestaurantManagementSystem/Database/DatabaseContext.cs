using Microsoft.EntityFrameworkCore;
using RestaurantManagementSystem.Areas.Chef.Models;
using RestaurantManagementSystem.Areas.Customer.Models;
using RestaurantManagementSystem.Areas.Staff.Models;
using RestaurantManagementSystem.Areas.StockManager.Models;
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
        public DbSet<Customers> customers { get; set; }
        public DbSet<AttendanceRecord> attendanceRecords { get; set; } 
        public DbSet<SMS> sms { get; set; }  
        public DbSet<StockDetails> stockDetails { get; set; }   
        public DbSet<Order> orders { get; set; }    
    }
}
