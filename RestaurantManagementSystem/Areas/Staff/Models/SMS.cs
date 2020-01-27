using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantManagementSystem.Areas.Staff.Models
{
    public class SMS
    {
        [Key]
        public int SMSId { get; set; }
        public string Sender { get; set; }
        public string Receiver { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; } 
        public Boolean ReadStatus { get; set; }
        


    }
}
