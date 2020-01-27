using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantManagementSystem.Areas.Chef.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        public int OrderTo { get; set; }
        public int OrderBy { get; set; }
        public int RequiredMaterialId { get; set; }
        public int Quantity { get; set; }
        public DateTime Date { get; set; }
        public Boolean ApprovedStatus { get; set; }  


    }
}
