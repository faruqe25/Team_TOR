using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantManagementSystem.Areas.Admin.ViewModels
{
    public class TableVm
    {
        public int Serial { get; set; }
        public int TableId { get; set; }
        [Required]
        public string TableNumber { get; set; }
        [Required]
        public int TableCapacity { get; set; }
        [Required]
        public int BookingPrice { get; set; }
        public Boolean BookedStatus { get; set; }
    }
}
