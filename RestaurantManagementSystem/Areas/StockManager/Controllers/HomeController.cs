using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RestaurantManagementSystem.Areas.Admin.Models;
using RestaurantManagementSystem.Areas.StockManager.Models;
using RestaurantManagementSystem.Areas.StockManager.ViewModels;
using RestaurantManagementSystem.Database;
using X.PagedList;

namespace RestaurantManagementSystem.Areas.StockManager.Controllers
{
    [Area("StockManager")]
    [Route("StockManager/[controller]/[action]")]
    [Authorize(Roles = "StockManager")]
    public class HomeController : Controller
    {
        private readonly DatabaseContext _context;

        public HomeController(DatabaseContext context)
        {
            _context = context;
        }
        public async Task<JsonResult> LoadChartData(int id)
        {
            var StockItemQuantity = from stock in await _context.StockDetails.
                                    Where(s => s.IngredientId == id).
                                     Where(a =>a.StockInDate.Month == DateTime.Now.Month)

                                     .ToListAsync()
                                    group stock by
                                    stock.StockInDate.Month into p
                                    let temp = (
                                          from val in p
                                          select new
                                          {
                                              Total = p.Sum(s => s.Quantity),
                                              Month = val.StockInDate.Month
                                          }
                                            )
                                    select temp;
            List<int> Quantity = new List<int>();
            int Month = 1;
            for (int i = 0; i < 12; i++)
            {
                try
                {
                    var Total = StockItemQuantity.ElementAt(0).ElementAt(0).Total;
                    var Months = StockItemQuantity.ElementAt(0).ElementAt(0).Month;
                    if (Months != Month)
                    {
                        Quantity.Add(0);
                    }
                    else
                    {
                        Quantity.Add(Total);
                    }

                }
                catch (Exception)
                {

                    Quantity.Add(0);
                }
                Month++;

            }
            

            return Json(Quantity);
        }
        public async Task<IActionResult> Index()
        {
            ViewBag.InredientList = await _context.Ingredient.AsNoTracking().ToListAsync();
            var StockItemQuantity = from stock in await _context.StockDetails.
                                    Where(s => s.IngredientId == 1)
                                    .Where(a => a.StockInDate.Month == DateTime.Now.Month)

                                     .ToListAsync()
                                    group stock by
                                    stock.StockInDate.Month into p
                                    let temp = (
                                          from val in p
                                          select new
                                          {
                                              Total = p.Sum(s => s.Quantity),
                                              Month = val.StockInDate.Month
                                          }
                                            )
                                    select temp;
            List<int> Quantity = new List<int>();
            int Month = 1;
            for (int i = 0; i < 12; i++)
            {
                try
                {
                    var Total = StockItemQuantity.ElementAt(0).ElementAt(0).Total;
                    var Months = StockItemQuantity.ElementAt(0).ElementAt(0).Month;
                    if (Months != Month)
                    {
                        Quantity.Add(0);
                    }
                    else
                    {
                        Quantity.Add(Total);
                    }
                    
                }
                catch (Exception)
                {

                    Quantity.Add(0);
                }
                Month++;

            }

            ViewBag.TotalQueantity = Quantity;
           

            return View();
        }
        public IActionResult AddStock()
        {
            var ingredientlist = _context.Ingredient.ToList();
            ViewBag.IngredientList = ingredientlist;
            return View();
        }
        [HttpPost]
        public IActionResult AddStock(StockDetailsVm stockdetailsvm)
        {
            var stck = _context.StockDetails.AsNoTracking().Where(s => s.IngredientId == stockdetailsvm.IngredientId).LastOrDefault();
            if (stck != null)
            {                
                stockdetailsvm.AvailableStock = stockdetailsvm.Quantity * 1000 + stck.AvailableStock;
            }
            else
            {
                stockdetailsvm.AvailableStock = stockdetailsvm.Quantity * 1000;
            }
            var ingredientlist = _context.Ingredient.ToList();
            ingredientlist.Insert(0, new Ingredient { IngredientId = 0, IngredientName = "Select Raw Ingredient" });
            ViewBag.IngredientList = ingredientlist;

            StockDetails stockdetails = new StockDetails()
            {
                IngredientId = stockdetailsvm.IngredientId,
                Quantity = stockdetailsvm.Quantity,
                StockInDate = DateTime.Now,
                AvailableStock= stockdetailsvm.AvailableStock,
            };
           
            _context.StockDetails.Add(stockdetails);
            _context.SaveChanges();
            ModelState.Clear();
            ViewBag.Success = "You have succesfully added.";
            return View();
        }
        public IActionResult StockDetails(int Page = 1)
        {
            var list = _context.StockDetails.AsNoTracking().Include(a=>a.Ingredient).ToList();
            var sent = new List<StockDetailsVm>();
            int c = 1;
            foreach (var item in list)
            {
                StockDetailsVm a = new StockDetailsVm()
                {
                    StockDetailsId = item.StockDetailsId,
                    Serial = c,
                    Quantity=item.Quantity,
                    AvailableStock=item.AvailableStock/1000,
                    IngredientName=item.Ingredient.IngredientName,
                    StockInDate=item.StockInDate
                
                };
                sent.Add(a);
                c++;

                
            }
            var list1 = sent.ToPagedList(Page, 5);
            return View(list1);
        }
        public IActionResult UpdateStock(int id)
        {
            var item = _context.StockDetails. 
                AsNoTracking().Where(a => a.StockDetailsId==id).FirstOrDefault();

            ViewBag.IngredientList = new SelectList(_context.Ingredient.ToList(), "IngredientId", "IngredientName");
            StockDetailsVm data = new StockDetailsVm()
            {
                StockDetailsId = item.StockDetailsId,
               
                Quantity = item.Quantity,
               
                IngredientId = item.IngredientId,
                StockInDate = item.StockInDate

            };
            return View(data);
        }
        [HttpPost]
        public IActionResult UpdateStock(StockDetailsVm stockdetailsvm)
        {
            StockDetails stockdetails = new StockDetails()
            {   
                StockDetailsId=stockdetailsvm.StockDetailsId,
                IngredientId = stockdetailsvm.IngredientId,
                Quantity = stockdetailsvm.Quantity,
                StockInDate = DateTime.Now,
                AvailableStock = stockdetailsvm.Quantity * 1000,
            };
            _context.StockDetails.Update(stockdetails);
            _context.SaveChanges();
            return RedirectToAction("StockDetails");
        }
        public async Task<IActionResult> StockStatus()
        {
            var stc = await _context.StockDetails.
                        AsNoTracking().Include(a => a.Ingredient).
                        ToListAsync();
            var data = stc.GroupBy(x => x.IngredientId).
                     Select(x => x.OrderByDescending(x => x.StockDetailsId).First());

            var sent = new List<StockDetailsVm>();
            foreach (var item in data)
            {
                StockDetailsVm p = new StockDetailsVm()
                {
                    StockInDate=item.StockInDate,
                    Quantity=item.Quantity,
                    AvailableStock=item.AvailableStock,
                    IngredientName=item.Ingredient.IngredientName,

                };
                sent.Add(p);
            }           
            return View(sent);
        }
    }
}

