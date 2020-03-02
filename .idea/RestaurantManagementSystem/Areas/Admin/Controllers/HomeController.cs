﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RestaurantManagementSystem.Areas.Admin.Models;
using RestaurantManagementSystem.Areas.Admin.ViewModels;
using RestaurantManagementSystem.Database;
using X.PagedList;

namespace RestaurantManagementSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class HomeController : Controller
    {
        private readonly DatabaseContext _context;

        public HomeController(DatabaseContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult SetMealHour()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SetMealHour(MealHourVm mealHourVm)
        {
            MealHour m = new MealHour()
            {
                MealHourId = 0,
                MealHourTitle = mealHourVm.MealHourTitle

            };
            _context.MealHour.Add(m);
            _context.SaveChanges();
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
            MealHour m = new MealHour()
            {
                MealHourId = mealHourVm.MealHourId,
                MealHourTitle = mealHourVm.MealHourTitle

            };
            _context.MealHour.Update(m);
            _context.SaveChanges();
            ModelState.Clear();
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
            ModelState.Clear();
            ViewBag.MealHour = new SelectList(_context.MealHour.AsNoTracking().
              ToList(), "MealHourId", "MealHourTitle");
            return View();
        }
        public IActionResult FoodItemList()
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
            return View(s);
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
            Ingredient s = new Ingredient()
            {
                IngredientId = a.IngredientId,
                IngredientName = a.IngredientName,
            };
            _context.Ingredient.Add(s);
            _context.SaveChanges();
            ModelState.Clear();
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
            var list = se.ToPagedList(Page, 4);
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

            //var tem = _context.RequiredMaterial.AsNoTracking().ToList();
            //var valid = false;
          
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
        //public IActionResult UpdateFoodRecipe(int id)
        //{
        //    var ls = _context.RequiredMaterial.AsNoTracking().
        //                Include(s => s.Ingredient).Include(s => s.FoodItem).
        //                Where(s => s.FoodItemId == id).ToList();



        //    var sent = new List<RequiredMaterialVm>();

        //    List<MaterialVm> prt = new List<MaterialVm>();
        //    int c = 1;
        //    foreach (var item in ls)
        //    {
        //        var ass = new RequiredMaterialVm();


        //        ass.Price = item.FoodItem.Price;
        //        ass.FoodItemId = item.FoodItemId;
        //        ass.FoodItemNames = item.FoodItem.FoodName;
        //        ass.RequiredMaterialId = item.RequiredMaterialId;
        //        MaterialVm t = new MaterialVm()
        //        {
        //            QuantityInGram = item.QuantityInGram,
        //            IngredientName = item.Ingredient.IngredientName
        //        };
        //        ass.MaterialVms.Add(t);
        //        ass.Serial = c;
        //        sent.Add(ass);
        //        c++;


        //    }


        //    return View(sent);


        //}
        //public IActionResult UpdateFoodRecipe(int id)
        //{
        //    var res = from ls in _context.RequiredMaterial
        //         .AsNoTracking().Include(s => s.FoodItem).
        //         Include(s => s.Ingredient).Where(s => s.FoodItemId == id)
        //              group ls by ls.FoodItemId into p
        //              let temp = (
        //                     from val in p
        //                     select new
        //                     {
        //                         Price = val.FoodItem.Price,
        //                         FoodItemId = val.FoodItem.FoodItemId,
        //                         FoodName = val.FoodItem.FoodName,
        //                         IngredientName = val.Ingredient.IngredientName,
        //                         Quantity = val.QuantityInGram,
        //                         IngredientId = val.IngredientId,
        //                         RequiredMaterialId = val.RequiredMaterialId

        //                     }
        //                     )
        //              select temp;

        //    var sent = new List<RequiredMaterialVm>();

        //    List<MaterialVm> prt = new List<MaterialVm>();
        //    int c = 1;
        //    foreach (var item in res)
        //    {
        //        var ass = new RequiredMaterialVm();
        //        foreach (var it in item)
        //        {

        //            ass.Price = it.Price;
        //            ass.RequiredMaterialId = it.RequiredMaterialId;
        //            ass.FoodItemId = it.FoodItemId;
        //            ass.FoodItemNames = it.FoodName;
        //            MaterialVm t = new MaterialVm()
        //            {
        //                QuantityInGram = it.Quantity,
        //                IngredientName = it.IngredientName,
        //                IngredientId = it.IngredientId
        //            };
        //            ass.MaterialVms.Add(t);
        //            ass.Serial = c;
        //        }
        //        sent.Add(ass);
        //        c++;


        //    }

        //    var raw = _context.Ingredient.AsNoTracking().ToList();
        //    ViewBag.RawItem = new SelectList(raw, "IngredientId", "IngredientName");
        //    return View(sent);
        //}
        //[HttpPost]
        //public IActionResult UpdateFoodRecipe(List<RequiredMaterialVm> ps)
        //{
        //    return View();
        //}



    }
}
        

 