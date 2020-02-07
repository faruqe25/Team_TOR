using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantManagementSystem.Areas.Admin.Models
{
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string Gender { get; set; }
        public string Photo { get; set; }
        public int Contact { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public DateTime JoiningDate { get; set; }
        public int DesignationId { get; set; }
    }
}
