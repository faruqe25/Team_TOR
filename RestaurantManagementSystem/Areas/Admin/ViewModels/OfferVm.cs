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
        public string Coupon { get; set; }
        public DateTime ValidatyStart { get; set; }
        public string ValidatyStart_ { get; set; }
        public DateTime ValidatyTo { get; set; }
        public string ValidatyTo_ { get; set; }
        [Range(1, 60)]
        public int Discount { get; set; }
    }
}
