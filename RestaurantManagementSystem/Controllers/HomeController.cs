using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantManagementSystem.Areas.Admin.Models;
using RestaurantManagementSystem.Areas.Admin.ViewModels;

using RestaurantManagementSystem.Areas.Customer.Models;

using RestaurantManagementSystem.Areas.Customer.ViewModels;
using RestaurantManagementSystem.Database;
using RestaurantManagementSystem.Helper;
using RestaurantManagementSystem.Models;

namespace RestaurantManagementSystem.Controllers
{
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

        public IActionResult Index()
        {
            var fooditemList = _context.FoodItems.
                AsNoTracking().Include(q => q.MealHour).ToList();
            var ReuiredMaterialAvailable = _context.RequiredMaterial.
                AsNoTracking().ToList();
            var fooditem = (from a in fooditemList
                           join b in ReuiredMaterialAvailable
                           on a.FoodItemId equals b.FoodItemId
                           select new
                           {
                               FoodName = a.FoodName,
                               MealHourName = a.MealHour.MealHourTitle,
                               Description = a.Description,
                               Price = a.Price,
                               FoodItemId = a.FoodItemId,
                           }).Distinct();
            var fooditemvmlist = new List<FoodItemVm>();
            foreach (var item in fooditem)
            {

                FoodItemVm fooditemvm = new FoodItemVm()
                {
                    FoodName = item.FoodName,
                    MealHourName = item.MealHourName,
                    Description = item.Description,
                    Price = item.Price,
                    FoodItemId = item.FoodItemId,
                };
                fooditemvmlist.Add(fooditemvm);
            }
            return View(fooditemvmlist);
        }
        [HttpGet]
        [HttpPost]
        public JsonResult CouponCheck(string Coupon)
        {
            HttpContext.Session.Remove("Discount");
            var Offr = _context.Offer.
              AsNoTracking().Where(s => s.Coupon == Coupon).FirstOrDefault();
            if(Offr!=null)
            {
                if(DateTime.Now.Date >= Offr.ValidatyStart.Date && DateTime.Now.Date <= Offr.ValidatyTo.Date)
                {
                   
                    HttpContext.Session.SetString("Discount", Offr.OfferId.ToString());
                    return Json(true);
                }
            }
            return Json(false);
        }
        [HttpGet]
        [HttpPost]
        public async Task<JsonResult> GetTableIfnoToolTip( int TableId)
        {
            var a = await _context.Table.AsNoTracking().
                Where(s => s.TableId == TableId).FirstOrDefaultAsync();
            return Json(a);

        }
        public async Task<JsonResult> GetDiscountByCoupon()
        {
            var a = HttpContext.Session.GetString("Discount");
            var Dis =await  _context.Offer.AsNoTracking().Where(s => s.OfferId == Convert.ToInt32(a)).FirstOrDefaultAsync();
            if(Dis==null)
            {
                return Json(0);
            }
            else
            {
                return Json(Dis.Discount);
            }
            
        }

        public JsonResult CartCurrentStatus()
        {
            float total = 0;
            var List = HttpContext.Session.Get<List<FoodCart>>("FoodS");
            if(List!=null)
            {
                 total = List.Sum(s => s.FoodPrice * s.Quantity);
            }
            else
            {
                 total = 0;
            }
            return Json(total);
        }

        [HttpGet]
        [HttpPost]
        public JsonResult SetCartValue(int id)
        {
            var foodDetails = _context.FoodItems.AsNoTracking().Where(s => s.FoodItemId == id).FirstOrDefault();
            var food = new FoodCart
            {
                FoodItemId = id,
                Quantity = 1,
                FoodName = foodDetails.FoodName,
                FoodPrice = foodDetails.Price,
                FoodDescription = foodDetails.Description
            };

            var List = HttpContext.Session.Get<List<FoodCart>>("FoodS");
            if (List == null)
            {
                List = new List<FoodCart>();
                List.Add(food);
            }
            else
            {

                var exist = List.Where(a => a.FoodItemId == id).FirstOrDefault();
                if (exist != null)
                {

                    food.Quantity = exist.Quantity + 1;
                    food.FoodItemId = id;
                    food.FoodPrice = food.FoodPrice;
                    List.Remove(exist);
                    List.Add(food);

                }
                else
                {
                    List.Add(food);
                }

            }


            HttpContext.Session.Set("FoodS", List);

            var count = HttpContext.Session.Get<List<FoodCart>>("FoodS").Sum(t => t.Quantity);

            return Json(count);
        }
        public JsonResult SetCartValueUpdated(int id, int Quantity)
        {


            var List = HttpContext.Session.Get<List<FoodCart>>("FoodS");
            var up = List.Where(s => s.FoodItemId == id).FirstOrDefault();
            var food = new FoodCart
            {
                FoodItemId = id,
                Quantity = Quantity,
                FoodName = up.FoodName,
                FoodPrice = up.FoodPrice,
                FoodDescription = up.FoodDescription
            };
            List.Remove(up);
            List.Add(food);

            HttpContext.Session.Set("FoodS", List);
            var total = List.Sum(s => s.FoodPrice * s.Quantity);

            return Json(total);
        }

        public JsonResult DeleteCart(int id)
        {
            var List = HttpContext.Session.Get<List<FoodCart>>("FoodS");
            var up = List.Where(s => s.FoodItemId == id).FirstOrDefault();
            List.Remove(up);
            HttpContext.Session.Set("FoodS", List);
            if (List != null)
            {
                var total = List.Sum(s => s.FoodPrice * s.Quantity);

                return Json(total);

            }
            return Json(0);
        }
        public JsonResult GetCartValueTotal()
        {
            var List = HttpContext.Session.Get<List<FoodCart>>("FoodS");
            var count = 0;
            if (List == null)
            {
                count = 0;
            }
            else
            {
                count = List.Count();
            }

            return Json(count);
        }
        [Authorize]
        public async Task<IActionResult> Cart()
        {
            var Exists = HttpContext.Session.Get<List<TableResevationCart>>("AvailableTable");
            if (Exists != null)
            {
                HttpContext.Session.Remove("AvailableTable");
            }
            var TableAvailableList = await _context.Table.AsNoTracking().Where(a => a.TableId != 1).ToListAsync();

            if (TableAvailableList.Count() != 0)
            {
                var Tables = new List<TableResevationCart>();
                foreach (var item in TableAvailableList)
                {
                    TableResevationCart tableResevationCart = new TableResevationCart()
                    {
                        TableId = item.TableId,
                        BookedStatus = item.BookedStatus,
                        TableName = item.TableNumber
                    };
                    Tables.Add(tableResevationCart);
                }
                HttpContext.Session.Set("AvailableTable", Tables);
            }

            var List = HttpContext.Session.Get<List<FoodCart>>("FoodS");
            if (List == null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}