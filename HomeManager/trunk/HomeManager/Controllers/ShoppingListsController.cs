using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HomeManager.Models;

namespace HomeManager.Controllers
{   
    public class ShoppingListsController : Controller
    {
        private HomeManagerContext context = new HomeManagerContext();

        //
        // GET: /ShoppingLists/

        public ViewResult Index()
        {
            return View(context.ShoppingLists.Include(shoppinglist => shoppinglist.ShoppingListItems).ToList());
        }

        //
        // GET: /ShoppingLists/Details/5

        public ViewResult Details(int id)
        {
            ShoppingList shoppinglist = context.ShoppingLists.Single(x => x.Id == id);
            return View(shoppinglist);
        }

        //
        // GET: /ShoppingLists/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /ShoppingLists/Create

        [HttpPost]
        public ActionResult Create(ShoppingList shoppinglist)
        {
            if (ModelState.IsValid)
            {
                context.ShoppingLists.Add(shoppinglist);
                context.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(shoppinglist);
        }
        
        //
        // GET: /ShoppingLists/Edit/5
 
        public ActionResult Edit(int id)
        {
            ShoppingList shoppinglist = context.ShoppingLists.Single(x => x.Id == id);
            return View(shoppinglist);
        }

        //
        // POST: /ShoppingLists/Edit/5

        [HttpPost]
        public ActionResult Edit(ShoppingList shoppinglist)
        {
            if (ModelState.IsValid)
            {
                context.Entry(shoppinglist).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(shoppinglist);
        }

        //
        // GET: /ShoppingLists/Delete/5
 
        public ActionResult Delete(int id)
        {
            ShoppingList shoppinglist = context.ShoppingLists.Single(x => x.Id == id);
            return View(shoppinglist);
        }

        //
        // POST: /ShoppingLists/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            ShoppingList shoppinglist = context.ShoppingLists.Single(x => x.Id == id);
            context.ShoppingLists.Remove(shoppinglist);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult CreateFromMenu(long startTimestamp, long endTimestamp)
        {
            DateTime startDate = startTimestamp.FromUnixTimestamp();
            DateTime endDate = endTimestamp.FromUnixTimestamp();

            List<Ingredient> ingredients = (from m in context.MenuItems
                                            from i in m.Recipe.Ingredients
                                            where m.Date >= startDate && m.Date <= endDate
                                            select i).ToList();

            ShoppingList shoppingList;

            var list = from l in context.ShoppingLists where l.Store == "Grocery" select l;

            if (list.Count() == 1)
            {
                shoppingList = list.Single();
            }
            else
            {
                shoppingList = new ShoppingList();

                shoppingList.Store = "Grocery";

                context.ShoppingLists.Add(shoppingList);
            }

            if (shoppingList.ShoppingListItems == null)
            {
                shoppingList.ShoppingListItems = new List<ShoppingListItem>();
            }

            int rank = shoppingList.ShoppingListItems.Count;

            foreach (Ingredient i in ingredients)
            {
                var existingItem = from item in shoppingList.ShoppingListItems where item.Unit == i.Unit && item.ItemId == i.ItemId select item;

                if (existingItem.Count() == 1)
                {
                    ShoppingListItem existing = existingItem.Single();

                    existing.Amount = (Mehroz.Fraction.ToFraction(existing.Amount) + Mehroz.Fraction.ToFraction(i.Amount)).ToDouble();
                }
                else
                {
                    ShoppingListItem sli = new ShoppingListItem();

                    sli.Rank = rank++;
                    sli.Amount = i.Amount;
                    sli.Item = i.Item;
                    sli.Unit = i.Unit;

                    context.ShoppingListItems.Add(sli);

                    shoppingList.ShoppingListItems.Add(sli);
                }
            }

            context.SaveChanges();

            return RedirectToAction("Details", new { id = shoppingList.Id });
        }

        public ActionResult Clear(int id)
        {
            var list = context.ShoppingLists.Where(l => l.Id == id).FirstOrDefault();

            if (list != null)
            {
                foreach (var item in list.ShoppingListItems.ToList())
                {
                    context.ShoppingListItems.Remove(item);
                }

                context.SaveChanges();
            }

            return RedirectToAction("Details", new { id = id });
        }
    }
}