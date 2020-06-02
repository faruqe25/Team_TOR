using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantManagementSystem.Areas.Admin.Models;
using RestaurantManagementSystem.Areas.Customer.Models;
using RestaurantManagementSystem.Areas.Customer.ViewModels;
using RestaurantManagementSystem.Areas.Manager.Models;
using RestaurantManagementSystem.Areas.StockManager.Models;
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
        [AllowAnonymous]
        public JsonResult TableReservationSet(DateTime From, DateTime To, int TableId)
        {
            HttpContext.Session.Remove("Table");
            TableResevationCart table = new TableResevationCart()
            {
                BookTimeFrom = From,
                BookTimeTo = To,
                Date = DateTime.Now,
                TableId = TableId
            };
            HttpContext.Session.Set("Table", table);
            return Json(true);
        }
        [AllowAnonymous]
        public async Task<JsonResult> GetTableName(int TableId)
        {
            var tb = await _context.Table.AsNoTracking().Where(a => a.TableId == TableId).FirstOrDefaultAsync();

            return Json(tb.TableNumber);
        }


        [HttpPost]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> Order()
        {
            var Discount = HttpContext.Session.GetString("Discount");
            var CustomerDetails = new Customers();
            var user = await userManager.GetUserAsync(User);
            var user1 = await userManager.FindByEmailAsync(User.Identity.Name);
            if (await userManager.IsInRoleAsync(user, "Customer") == true)
            {
                CustomerDetails = _context.Customers.
                            Where(s => s.MobileNumber == user.PhoneNumber)
                            .FirstOrDefault();
            }
            var ReservedTable = HttpContext.Session.Get<TableResevationCart>("Table");
            if (ReservedTable != null)
            {
                CustomerOrderedTable abc = new CustomerOrderedTable()
                {

                    CustomerOrderedTableId = 0,
                    CustomersId = CustomerDetails.CustomersId
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
                        CustomerOrderDetails ab = new CustomerOrderDetails()
                        {
                            CustomerOrderDetailsId = 0,
                            FoodItemId = item.FoodItemId,
                            Quantity = item.Quantity,
                            
                            PaymentStatus = false,
                            CustomerOrderedTableId = abc.CustomerOrderedTableId,
                        };
                        if(Discount!=null)
                        {
                            ab.OfferId = Convert.ToInt32(Discount);
                        }
                        await _context.CustomerOrderDetails.AddAsync(ab);
                        await _context.SaveChangesAsync();
                    }
                }
            }
            else
            {
                CustomerOrderedTable tabless = new CustomerOrderedTable()
                {
                    CustomerOrderedTableId = 0,
                    CustomersId = CustomerDetails.CustomersId,
                    TableId = 1,

                    Date=DateTime.Now
                 };

                await _context.CustomerOrderedTable.AddAsync(tabless);
                await _context.SaveChangesAsync();


                var orderlist = HttpContext.Session.Get<List<FoodCart>>("FoodS");
                if (orderlist != null)
                {
                    foreach (var item in orderlist)
                    {

                        CustomerOrderDetails ab = new CustomerOrderDetails()
                        {
                            CustomerOrderDetailsId = 0,
                            FoodItemId = item.FoodItemId,
                            Quantity = item.Quantity,
                            
                            PaymentStatus = false,
                            CustomerOrderedTableId = tabless.CustomerOrderedTableId

                        };
                        if (Discount != null)
                        {
                            ab.OfferId = Convert.ToInt32(Discount);
                        }
                        await _context.CustomerOrderDetails.AddAsync(ab);
                        await _context.SaveChangesAsync();
                    }
                }
            }

            var FoodItemList = HttpContext.Session.Get<List<FoodCart>>("FoodS");
            foreach (var item in FoodItemList)
            {
                var IngredientList = await _context.RequiredMaterial
                        .AsNoTracking().Where(a => a.FoodItemId == item.FoodItemId).ToListAsync();
                for (int i = 0; i < IngredientList.Count(); i++)
                {
                    var Requiered = await _context.RequiredMaterial.AsNoTracking()
                          .Where(a => a.RequiredMaterialId == IngredientList[i].RequiredMaterialId)
                          .FirstOrDefaultAsync();

                    var NeedToUpdateMaterials = await _context.StockDetails.
                        AsNoTracking().Where(a => a.IngredientId == IngredientList[i].IngredientId)
                        .LastOrDefaultAsync();

                    for (int j = 0; j < item.Quantity; j++)
                    {

                        var quaantity = NeedToUpdateMaterials.AvailableStock - Requiered.QuantityInGram;
                        NeedToUpdateMaterials.AvailableStock = quaantity;
                        _context.StockDetails.Update(NeedToUpdateMaterials);
                        await _context.SaveChangesAsync();
                        _context.Entry<StockDetails>(NeedToUpdateMaterials).State = EntityState.Detached;

                    }
                    NeedToUpdateMaterials = new StockDetails();
                    Requiered = new RequiredMaterial();
                }

                IngredientList = new List<RequiredMaterial>();
            }



            HttpContext.Session.Remove("FoodS");
            HttpContext.Session.Remove("Table");
            HttpContext.Session.Remove("Discount");

            return RedirectToAction("Index", "Home");
        }

        
    }
}