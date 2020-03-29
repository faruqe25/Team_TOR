using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantManagementSystem.Areas.Admin.ViewModels
{
    public class OfferVm
    {
        public int Serial { get; set; }
        public int OfferId { get; set; }
        [Required]
        public string Coupon { get; set; }
        [Required]
        public DateTime ValidatyStart { get; set; }
        
        public string ValidatyStart_ { get; set; }
        [Required]
        public DateTime ValidatyTo { get; set; }
        public string ValidatyTo_ { get; set; }
        [Range(1, 60)]
        [Required]
        public int Discount { get; set; }
    }
}
