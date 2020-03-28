using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantManagementSystem.Areas.Manager.ViewModels
{
    public class TempSell
    {
        public int Day { get; set; }
        public float Total { get; set; } 
        public int Quantity { get; set; }  
        public DateTime Date { get; set; }   
        public string FoodName { get; set; }   
        public string CustomerName { get; set; }   
        public float FoodPrice { get; set; }    
    }
}
