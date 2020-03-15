using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public JsonResult TableReservationSet(DateTime From,DateTime To,int TableId)  
        {
                HttpContext.Session.Remove("Table");
                TableResevationCart table = new TableResevationCart() {
                    BookTimeFrom = From,
                    BookTimeTo = To,
                    Date = DateTime.Today,
                    TableId=TableId
                };
                HttpContext.Session.Set("Table", table);
                return Json(true);
        }
        public async Task<JsonResult> GetTableName(int TableId)  
        {
            var tb = await _context.Table.AsNoTracking().Where(a => a.TableId == TableId).FirstOrDefaultAsync();
               
            return Json(tb.TableNumber);
        }


        [HttpPost]
        public async Task<IActionResult> Order(CustomerOrderedTable ct) 
        {   var email = "";
            var CustomerDetails=new Customers();
            if (signInManager.IsSignedIn(User)) {  email = User.Identity.Name; }
            var user = await userManager.FindByEmailAsync(email);
            if (await userManager.IsInRoleAsync(user, "Customer") == true) {
                CustomerDetails = _context.Customers.
                            Where(s => s.MobileNumber == user.PhoneNumber)
                            .FirstOrDefault();
            }
            var ReservedTable = HttpContext.Session.Get<TableResevationCart>("Table");
            if (ReservedTable != null)
            {
                CustomerOrderedTable abc = new CustomerOrderedTable()
                {
                    CustomerOrderedTableId = ct.CustomerOrderedTableId,
                    CustomersId = CustomerDetails.CustomersId,
                };
                abc.BookTimeFrom = ReservedTable.BookTimeFrom;
                abc.BookTimeTo = ReservedTable.BookTimeTo;
                abc.Date = ReservedTable.Date;
                abc.TableId = ReservedTable.TableId;
                await _context.CustomerOrderedTable.AddAsync(abc);
                await _context.SaveChangesAsync();
                var update = _context.Table.Where(a => a.TableId == ReservedTable.TableId).FirstOrDefault();
                   update.BookedStatus = true;
                   _context.Table.Update(update);
                await _context.SaveChangesAsync();
                
                var orderlist = HttpContext.Session.Get<List<FoodCart>>("FoodS");
                if (orderlist != null)
                {
                    foreach (var item in orderlist)
                    {
                        var countfood = orderlist.Where(s => s.FoodItemId == item.FoodItemId).Count();
                        CustomerOrderDetails ab = new CustomerOrderDetails()
                        {
                            CustomerOrderDetailsId = 0,
                            FoodItemId = item.FoodItemId,
                            Quantity = countfood,
                            DiscountId = 0,
                            OnlineStatus = true,
                            CustomerOrderedTableId = abc.CustomerOrderedTableId,
                        };
                        await _context.CustomerOrderDetails.AddAsync(ab);
                        await _context.SaveChangesAsync();
                    }
                }
            }
            else
            {

                var orderlist = HttpContext.Session.Get<List<FoodCart>>("FoodS");
                if (orderlist != null)
                {
                    foreach (var item in orderlist)
                    {
                        var countfood = orderlist.Where(s => s.FoodItemId == item.FoodItemId).Count();
                        CustomerOrderDetails ab = new CustomerOrderDetails()
                        {
                            CustomerOrderDetailsId = 0,
                            FoodItemId = item.FoodItemId,
                            Quantity = countfood,
                            DiscountId = 0,
                            OnlineStatus = true,
                            
                        };
                        await _context.CustomerOrderDetails.AddAsync(ab);
                        await _context.SaveChangesAsync();
                    }
                }
            }
            return View();
        }
    }
}