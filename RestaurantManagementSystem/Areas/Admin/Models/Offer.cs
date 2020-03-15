using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantManagementSystem.Areas.Admin.Models
{
    public class Offer
    {
        [Key]
        public int OfferId { get; set; }
        public string Coupon { get; set; }
        public DateTime ValidatyStart{ get; set; }
        public DateTime ValidatyTo{ get; set; }
        public  int Discount  { get; set; }      

    }
}
