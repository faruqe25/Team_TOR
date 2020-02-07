using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantManagementSystem.Areas.Admin.Models
{
    public class EmployeeDesignation
    {
        [Key]
        public int EmployeeDesignationId { get; set; }

        public int EmployeeDesignationTitle { get; set; }
        public double Salary { get; set; }
    }
}
