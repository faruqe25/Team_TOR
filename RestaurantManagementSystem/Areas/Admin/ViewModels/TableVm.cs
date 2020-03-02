using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantManagementSystem.Areas.Admin.ViewModels
{
    public class TableVm
    {
        public int Serial { get; set; }
        public int TableId { get; set; }
        public string TableNumber { get; set; }
        public int TableCapacity { get; set; }
        public int BookingPrice { get; set; }
        public Boolean BookedStatus { get; set; }
    }
}
