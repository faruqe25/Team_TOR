using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantManagementSystem.Areas.Manager.ViewModels
{
    public class Invoice
    {
        public int Quantity { get; set; }
        public string FoodName { get; set; } 
        public float Price { get; set; }  
        public int  Discount { get; set; }  
        public string  Coupone   { get; set; }   
        public float  Total   { get; set; }  
        public float  TablePrice   { get; set; }   

    }
}
