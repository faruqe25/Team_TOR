using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagementSystem.Areas.Admin.Models;
using RestaurantManagementSystem.Areas.Customer.Models;
using RestaurantManagementSystem.Areas.Customer.ViewModels;
using RestaurantManagementSystem.Areas.Manager.Models;
using RestaurantManagementSystem.Database;
using RestaurantManagementSystem.Helper;

namespace RestaurantManagementSystem.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Route("Customer/[controller]/[action]")]
    public class HomeController : Controller
    {
        public readonly DatabaseContext _context;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public HomeController(DatabaseContext context,
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.roleManager = roleManager;

        }
        [HttpPost]
        public async Task<IActionResult> Order(CustomerOrderedTable ct) 
        {   var email = "";
            var CustrId=new Customers();
            if (signInManager.IsSignedIn(User)) {  email = User.Identity.Name; }
            var user = await userManager.FindByEmailAsync(email);
            if (await userManager.IsInRoleAsync(user, "Customer") == true) {
                CustrId = _context.Customers.Where(s => s.MobileNumber == user.PhoneNumber).FirstOrDefault();
            }

            CustomerOrderedTable abc = new CustomerOrderedTable
            {
                CustomerOrderedTableId=ct.CustomerOrderedTableId,
                CustomersId=CustrId.CustomersId,
                BookTimeFrom=ct.BookTimeFrom,
                BookTimeTo=ct.BookTimeTo,
                Date=ct.Date,
                ConfirmStatus=false,
                TableId=ct.TableId, 
            };
            await _context.CustomerOrderedTable.AddAsync(abc);
            await _context.SaveChangesAsync();
            var orderlist =HttpContext.Session.Get<List<FoodCart>>("FoodS");
            if(orderlist!=null)
            {
                foreach (var item in orderlist)
                {
                    var countfood = orderlist.Where(s => s.FoodItemId == item.FoodItemId).Count();
                    CustomerOrderDetails ab = new CustomerOrderDetails() 
                    {
                       CustomerOrderDetailsId=0,
                       FoodItemId=item.FoodItemId,
                       Quantity= countfood,
                       DiscountId=0,
                       OnlineStatus=true,
                       CustomerOrderedTableId=abc.CustomerOrderedTableId,
                    };
                    await _context.CustomerOrderDetails.AddAsync(ab);
                    await _context.SaveChangesAsync();
                }
            }
            return View();
        }
    }
}