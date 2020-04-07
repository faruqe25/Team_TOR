 using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RestaurantManagementSystem.Areas.Admin.Models;
using RestaurantManagementSystem.Areas.Admin.ViewModels;
using RestaurantManagementSystem.Areas.Manager.ViewModels;
using RestaurantManagementSystem.Database;
using X.PagedList;

namespace RestaurantManagementSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    [Authorize(Roles ="Admin")]
    public class HomeController : Controller
    {
        private readonly DatabaseContext _context;
        public HomeController(DatabaseContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var SellSRecord = from sells in await _context.CustomerOrderDetails.AsNoTracking()
                                    .Include(s => s.CustomerOrderedTable).Include(s => s.FoodItem)
                                    .Where(s => s.PaymentStatus == true)
                                    .Where(a => a.CustomerOrderedTable.Date.Month == DateTime.Now.Month)
                                    .ToListAsync()
                              group sells by
                              sells.CustomerOrderedTable.Date.Day into p
                              let temp = (
                                    from val in p
                                    select new
                                    {
                                        Total = p.Sum(s => s.Quantity * s.FoodItem.Price),
                                        Day = p.Key
                                    }
                                     )
                              select temp;
            List<float> TotalSells = new List<float>();
            List<string> Day = new List<string>();
            var s = new List<TempSell>();

            foreach (var item in SellSRecord)
            {

                foreach (var t in item)
                {
                    var p = new TempSell()
                    {
                        Day = t.Day,
                        Total = t.Total
                    };
                    s.Add(p);
                    break;
                }
            }
            var data = s.OrderBy(s => s.Day);
            for (int i = 0; i < 31; i++)
            {
                TotalSells.Add(0);
            }
            foreach (var item in data)
            {
                var index = item.Day - 1;
                var total = item.Total;
                TotalSells[index] = total;
            }
            ViewBag.TotalFoodSells = TotalSells;
            return View();
        }
        public IActionResult SetMealHour()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SetMealHour(MealHourVm mealHourVm)
        {
            var valid = _context.MealHour.AsNoTracking().
                Where(t => t.MealHourTitle == mealHourVm.MealHourTitle).FirstOrDefault();
            if (valid != null)
            {
                ViewBag.Validation = "You have already added " + mealHourVm.MealHourTitle + ".";
                return View();
            }
            MealHour m = new MealHour()
            {
                MealHourId = 0,
                MealHourTitle = mealHourVm.MealHourTitle
            };
            _context.MealHour.Add(m);
            _context.SaveChanges();
            ViewBag.Success = "You have succesfully added " + mealHourVm.MealHourTitle + ".";
            ModelState.Clear();
            return View();
        }
        public IActionResult MealHourInfo()
        {
            var s = _context.MealHour.AsNoTracking().ToList();
            var a = new List<MealHourVm>();
            int c = 1;
            foreach (var item in s)
            {
                MealHourVm sp = new MealHourVm()
                {
                    Serial = c,
                    MealHourId = item.MealHourId,
                    MealHourTitle = item.MealHourTitle
                };
                a.Add(sp);
                c++;

            }

            return View(a);
        }
        public IActionResult UpdateMealHour(int id)
        {

            var mealhourVm = _context.MealHour.AsNoTracking()
                 .Where(t => t.MealHourId == id).FirstOrDefault();
            MealHourVm m = new MealHourVm()
            {
                MealHourId = mealhourVm.MealHourId,
                MealHourTitle = mealhourVm.MealHourTitle

            };
            return View(m);
        }
        [HttpPost]
        public IActionResult UpdateMealHour(MealHourVm mealHourVm)
        {
            var valid = _context.MealHour.AsNoTracking().
                Where(t => t.MealHourTitle == mealHourVm.MealHourTitle).FirstOrDefault();
            if (valid != null)
            {
                ViewBag.Validation = "You have already added.";
                return View();
            }
            MealHour m = new MealHour()
            {
                MealHourId = mealHourVm.MealHourId,
                MealHourTitle = mealHourVm.MealHourTitle
            };
            _context.MealHour.Update(m);
            _context.SaveChanges();
            ModelState.Clear();
            //ViewBag.Success = "You have succesfully updated.";
            return RedirectToAction("MealHourInfo");
        }
        public IActionResult DeleteMealHour(int id)
        {
            var mealhourVm = _context.MealHour.AsNoTracking()
                    .Where(t => t.MealHourId == id).FirstOrDefault();
            _context.MealHour.Remove(mealhourVm);
            _context.SaveChanges();
            return RedirectToAction("MealHourInfo");
        }
        public IActionResult AddFoodItem()
        {
            ViewBag.MealHour = new SelectList(_context.MealHour.AsNoTracking().
                ToList(), "MealHourId", "MealHourTitle");
            return View();
        }
        [HttpPost]
        public IActionResult AddFoodItem(FoodItemVm a)
        {
            var valid = _context.FoodItems.AsNoTracking().
                Where(t => t.FoodName == a.FoodName).FirstOrDefault();
            if (valid != null)
            {
                ViewBag.Validation = "You have already added " + a.FoodName + ".";
                ViewBag.MealHour = new SelectList(_context.MealHour.AsNoTracking().
                   ToList(), "MealHourId", "MealHourTitle");
                return View();
                //RedirectToAction("AddFoodItem");
            }
            FoodItem p = new FoodItem()
            {
                FoodName = a.FoodName,
                Description = a.Description,
                Price = a.Price,
                FoodItemId = a.FoodItemId,
                MealHourId = a.MealHourId,
            };
            _context.FoodItems.Add(p);
            _context.SaveChanges();
            ViewBag.Success = "You have succesfully added " + a.FoodName + ".";
            ModelState.Clear();
            ViewBag.MealHour = new SelectList(_context.MealHour.AsNoTracking().
              ToList(), "MealHourId", "MealHourTitle");
            return View();
        }
        public IActionResult FoodItemList(int Page=1)
        {
            var a = _context.FoodItems.AsNoTracking().Include(s => s.MealHour).ToList();
            var s = new List<FoodItemVm>();
            int c = 1;
            foreach (var item in a)
            {
                FoodItemVm ab = new FoodItemVm()
                {
                    Serial = c,
                    FoodName = item.FoodName,
                    MealHourName = item.MealHour.MealHourTitle,
                    Price = item.Price,
                    Description = item.Description,
                    FoodItemId = item.FoodItemId

                };
                s.Add(ab);
                c++;
            }
            var list = s.ToPagedList(Page, 5);
            return View(list);
            
        }
        public IActionResult UpdateFoodItem(int id)
        {
            var foodItem = _context.FoodItems.AsNoTracking()
                 .Where(t => t.FoodItemId == id).FirstOrDefault();
            FoodItemVm m = new FoodItemVm()
            {
                FoodItemId = foodItem.FoodItemId,
                Price = foodItem.Price,
                Description = foodItem.Description,
                MealHourId = foodItem.MealHourId,
                FoodName = foodItem.FoodName

            };
            ViewBag.MealHour = new SelectList(_context.MealHour.AsNoTracking().
                ToList(), "MealHourId", "MealHourTitle");
            return View(m);
        }
        [HttpPost]
        public IActionResult UpdateFoodItem(FoodItemVm a)
        {
            

            FoodItem p = new FoodItem()
            {
                FoodName = a.FoodName,
                Description = a.Description,
                Price = a.Price,
                FoodItemId = a.FoodItemId,
                MealHourId = a.MealHourId,

            };
            _context.FoodItems.Update(p);
            _context.SaveChanges();
            ModelState.Clear();
            return RedirectToAction("FoodItemList");

        }
        public IActionResult DeleteFoodItem(int id)
        {
            var foodItem = _context.FoodItems.AsNoTracking()
                 .Where(t => t.FoodItemId == id).FirstOrDefault();
            _context.FoodItems.Remove(foodItem);
            _context.SaveChanges();
            return RedirectToAction("FoodItemList");
        }
        public IActionResult AddIngredient()
        {

            return View();
        }
        [HttpPost]
        public IActionResult AddIngredient(IngredientVm a)
        {
            var valid = _context.Ingredient.AsNoTracking().
                Where(t => t.IngredientName == a.IngredientName).FirstOrDefault();
            if (valid != null)
            {
                ViewBag.Validation = "You have already added " + a.IngredientName;
                return View();
            }
            Ingredient s = new Ingredient()
            {
                IngredientId = a.IngredientId,
                IngredientName = a.IngredientName,
            };
            _context.Ingredient.Add(s);
            _context.SaveChanges();
            ModelState.Clear();
            ViewBag.Success = "You have succesfully added " + a.IngredientName;
            return View();
        }
        public IActionResult IngredientList(int Page = 1)
        {
            var li = _context.Ingredient.AsNoTracking().ToList();
            var se = new List<IngredientVm>();
            int c = 1;
            foreach (var item in li)
            {
                IngredientVm s = new IngredientVm()
                {
                    IngredientId = item.IngredientId,
                    IngredientName = item.IngredientName,
                    Serial = c,
                };
                se.Add(s);
                c++;
            }
            var list = se.ToPagedList(Page, 5);
            return View(list);
        }

        public IActionResult UpdateIngredient(int id)
        {
            var a = _context.Ingredient.AsNoTracking()
                .Where(k => k.IngredientId == id).FirstOrDefault();
            IngredientVm s = new IngredientVm()
            {
                IngredientId = a.IngredientId,
                IngredientName = a.IngredientName,
            };

            return View(s);
        }
        [HttpPost]
        public IActionResult UpdateIngredient(IngredientVm st)
        {
            var valid = _context.Ingredient.AsNoTracking().
                Where(t => t.IngredientName == st.IngredientName).FirstOrDefault();
            if (valid != null)
            {
                ViewBag.Validation = "You have already added " + st.IngredientName;
                return View();
            }
            Ingredient s = new Ingredient()
            {
                IngredientId = st.IngredientId,
                IngredientName = st.IngredientName,
            };
            _context.Ingredient.Update(s);
            _context.SaveChanges();
            return RedirectToAction("IngredientList");
        }
        public IActionResult DeleteIngredient(int id)
        {
            var a = _context.Ingredient.AsNoTracking()
                .Where(k => k.IngredientId == id).FirstOrDefault();
            _context.Ingredient.Remove(a);
            _context.SaveChanges();

            return RedirectToAction("IngredientList");
        }
        public JsonResult GetIngredients()
        {
            var ingre = _context.Ingredient.AsNoTracking().ToList();
            var slist = new SelectList(ingre, "IngredientId", "IngredientName");

            return Json(slist);
        }

        public IActionResult SetFoodRecipe()
        {
            var raw = _context.FoodItems.AsNoTracking().ToList();
            ViewBag.RawItem = new SelectList(raw, "FoodItemId", "FoodName");
            return View();
        }
        [HttpPost]
        public IActionResult SetFoodRecipe(RequiredMaterialVm Rl)
        {
            var raw = _context.FoodItems.AsNoTracking().ToList();
            ViewBag.RawItem = new SelectList(raw, "FoodItemId", "FoodName");

           

            foreach (var item in Rl.MaterialVms)
            {
                RequiredMaterial a = new RequiredMaterial();
                a.RequiredMaterialId = Rl.RequiredMaterialId;
                a.FoodItemId = Rl.FoodItemId;
                a.IngredientId = item.IngredientId;
                a.QuantityInGram = item.QuantityInGram;
                _context.RequiredMaterial.Add(a);
                _context.SaveChanges();
            }
            ViewBag.Success = "You have succesfully added food recipe.";
            ModelState.Clear();
            return View();
        }
        public IActionResult FoodRecipeDetails()
        {
            var res = from ls in _context.RequiredMaterial
                 .AsNoTracking().Include(s => s.FoodItem).
                 Include(s => s.Ingredient)
                      group ls by ls.FoodItemId into p
                      let temp = (
                             from val in p
                             select new
                             {
                                 Price = val.FoodItem.Price,
                                 FoodItemId = val.FoodItem.FoodItemId,
                                 FoodName = val.FoodItem.FoodName,
                                 IngredientName = val.Ingredient.IngredientName,
                                 Quantity = val.QuantityInGram,
                                 IngredientId = val.IngredientId,
                                 RequiredMaterialId = val.RequiredMaterialId

                             }
                             )
                      select temp;

            var sent = new List<RequiredMaterialVm>();


            List<MaterialVm> prt = new List<MaterialVm>();
            int c = 1;
            foreach (var item in res)
            {
                var ass = new RequiredMaterialVm();
                foreach (var it in item)
                {


                    ass.Price = it.Price;
                    ass.FoodItemId = it.FoodItemId;
                    ass.FoodItemNames = it.FoodName;
                    MaterialVm t = new MaterialVm()
                    {
                        QuantityInGram = it.Quantity,
                        IngredientName = it.IngredientName

                    };
                    ass.MaterialVms.Add(t);
                    ass.Serial = c;
                }
                sent.Add(ass);
                c++;

            }

            return View(sent);
        }
        public IActionResult DeleteFoodRecipe(int id)
        {
            var ls = _context.RequiredMaterial.AsNoTracking().Where(s => s.FoodItemId == id).ToList();
            _context.RequiredMaterial.RemoveRange(ls);
            _context.SaveChanges();

            return RedirectToAction("FoodRecipeDetails");

        }
        public IActionResult AddNewOffer()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddNewOffer(OfferVm offervm)
        {
            var valid = _context.Offer.AsNoTracking().
               Where(t => t.Coupon == offervm.Coupon).FirstOrDefault();
            if (valid != null)
            {
                ViewBag.Validation = "You have already added Coupon " + offervm.Coupon + ".";
                return View();
            }
            Offer offer = new Offer()
            {
                Coupon = offervm.Coupon,
                Discount = offervm.Discount,
                ValidatyStart = offervm.ValidatyStart,
                ValidatyTo = offervm.ValidatyTo
            };
            _context.Offer.Add(offer);
            _context.SaveChanges();
            ViewBag.Success = "You have succesfully added Coupon " + offervm.Coupon + ".";
            ModelState.Clear();
            return View();
        }
        public IActionResult OfferDetails(int Page=1)
        {
            var offerdetails = _context.Offer.AsNoTracking().ToList();
            var offerdetailslist = new List<OfferVm>();
            int count = 1;
            foreach (var item in offerdetails)
            {
                OfferVm offervm = new OfferVm()
                {
                    Serial = count,
                    OfferId = item.OfferId,
                    Coupon = item.Coupon,
                    Discount = item.Discount,
                    ValidatyStart_ = item.ValidatyStart.ToShortDateString(),
                    ValidatyTo_ = item.ValidatyTo.ToShortDateString()
                };
                offerdetailslist.Add(offervm);
                count++;
            }
            var sent = offerdetailslist.ToPagedList(Page, 5);
            return View(sent);
        }
        public IActionResult UpdateFoodOffer(int id)
        {
            var offer = _context.Offer.AsNoTracking().Where(q => q.OfferId == id).FirstOrDefault();
            OfferVm offervm = new OfferVm()
            {
                OfferId = offer.OfferId,
                Coupon = offer.Coupon,
                Discount = offer.Discount,
                ValidatyStart = offer.ValidatyStart,
                ValidatyTo = offer.ValidatyTo
            };
            return View(offervm);
        }
        [HttpPost]
        public IActionResult UpdateFoodOffer(OfferVm offervm)
        {
           
            Offer offer = new Offer()
            {
                OfferId = offervm.OfferId,
                Coupon = offervm.Coupon,
                Discount = offervm.Discount,
                ValidatyStart = offervm.ValidatyStart,
                ValidatyTo = offervm.ValidatyTo
            };
            _context.Offer.Update(offer);
            _context.SaveChanges();
            ModelState.Clear();
            return RedirectToAction("OfferDetails");
        }
        public IActionResult DeleteFoodOffer(int id)
        {
            var offer = _context.Offer.AsNoTracking().Where(q => q.OfferId == id).FirstOrDefault();
            _context.Offer.Remove(offer);
            _context.SaveChanges();
            return RedirectToAction("OfferDetails");
        }
        public IActionResult AddNewTable()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddNewTable(TableVm tablevm)
        {
            var valid = _context.Table.AsNoTracking().
               Where(t => t.TableNumber == tablevm.TableNumber).FirstOrDefault();
            if (valid != null)
            {
                ViewBag.Validation = "You have already added " + tablevm.TableNumber + ".";
                return View();
            }
            Table table = new Table
            {
                TableNumber = tablevm.TableNumber,
                TableCapacity = tablevm.TableCapacity,
                BookingPrice = tablevm.BookingPrice
            };
            _context.Table.Add(table);
            _context.SaveChanges();
            ViewBag.Success = "You have succesfully added " + tablevm.TableNumber + ".";
            ModelState.Clear();
            return View();
        }
        public IActionResult TableList(int Page=1)
        {
            var tablelist = _context.Table.AsNoTracking().Where(s=>s.TableId!=1).ToList();
            var tablelistvm = new List<TableVm>();
            int count = 1;
            foreach (var item in tablelist)
            {
                TableVm tablevm = new TableVm()
                {
                    Serial = count,
                    TableId = item.TableId,
                    TableNumber = item.TableNumber,
                    TableCapacity = item.TableCapacity,
                    BookingPrice = item.BookingPrice
                };
                count++;
                tablelistvm.Add(tablevm);
            }
            var sent = tablelistvm.ToPagedList(Page, 5);
            return View(sent);
        }
        public IActionResult UpdateTableInfo(int id)
        {
            var check = _context.Table.Where(q => q.TableId == id).AsNoTracking().FirstOrDefault();
            TableVm tablevm = new TableVm()
            {
                TableId = id,
                TableNumber = check.TableNumber,
                TableCapacity = check.TableCapacity,
                BookingPrice = check.BookingPrice
            };
            return View(tablevm);
        }
        [HttpPost]
        public IActionResult UpdateTableInfo(TableVm tablevm)
        {
            
            Table table = new Table()
            {
                TableId = tablevm.TableId,
                TableNumber = tablevm.TableNumber,
                TableCapacity = tablevm.TableCapacity,
                BookingPrice = tablevm.BookingPrice
            };
            _context.Table.Update(table);
            _context.SaveChanges();
            return RedirectToAction("TableList");
        }
        public IActionResult RemoveTableInfo(int id)
        {
            var tableinfo = _context.Table.Where(q => q.TableId == id).AsNoTracking().FirstOrDefault();
            _context.Table.Remove(tableinfo);
            _context.SaveChanges();
            return RedirectToAction("TableList");
        }
        public async Task<IActionResult>TotalSells(int Page=1)
        {


            var SellSRecord = from sells in await _context.CustomerOrderDetails.AsNoTracking()
                                    .Include(s => s.CustomerOrderedTable).Include(s => s.FoodItem)
                                    .Where(s => s.PaymentStatus == true)
                                    .ToListAsync()
                              group sells by
                              sells.CustomerOrderedTable.Date.Day into ps
                              let temp = (
                                    from val in ps
                                    select new
                                    {
                                        Total = ps.Sum(s => s.Quantity * s.FoodItem.Price),
                                        Day = ps.Key,
                                        Quantity= ps.Sum(s => s.Quantity),
                                        Data=val.CustomerOrderedTable.Date

                                    }
                                     )
                              select temp;
           
            var s = new List<TempSell>();

            foreach (var item in SellSRecord)
            {

                foreach (var t in item)
                {
                    var pa = new TempSell()
                    {
                        Day = t.Day,
                        Total = t.Total,
                        Quantity=t.Quantity,
                        Date=t.Data
                    };
                    s.Add(pa);
                    break;
                }
            }


            var p = s.OrderByDescending(s => s.Date);
            var list1 = p.ToPagedList(Page, 5);
            return View(list1);
           


        }
    }
    
}
        

 