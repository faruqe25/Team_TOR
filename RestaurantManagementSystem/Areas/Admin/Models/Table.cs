using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantManagementSystem.Areas.Admin.Models
{
    public class Table
    {
       
        [Key]
        public int TableId { get; set; }
        public string TableNumber { get; set; }
        public int TableCapacity { get; set; }
        public int BookingPrice { get; set; }
        public Boolean BookedStatus { get; set; }
    }
}
