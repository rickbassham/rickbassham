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
    public class ShoppingListItemsController : Controller
    {
        private HomeManagerContext context = new HomeManagerContext();

        //
        // GET: /ShoppingListItems/

        public ViewResult Index()
        {
            return View(context.ShoppingListItems.Include(shoppinglistitem => shoppinglistitem.ShoppingList).Include(shoppinglistitem => shoppinglistitem.Item).ToList());
        }

        //
        // GET: /ShoppingListItems/Details/5

        public ViewResult Details(int id)
        {
            ShoppingListItem shoppinglistitem = context.ShoppingListItems.Single(x => x.Id == id);
            return View(shoppinglistitem);
        }

        //
        // GET: /ShoppingListItems/Create

        public ActionResult Create()
        {
            ViewBag.PossibleShoppingLists = context.ShoppingLists;
            ViewBag.PossibleItems = context.Items;
            return View();
        } 

        //
        // POST: /ShoppingListItems/Create

        [HttpPost]
        public ActionResult Create(ShoppingListItem shoppinglistitem)
        {
            if (ModelState.IsValid)
            {
                context.ShoppingListItems.Add(shoppinglistitem);
                context.SaveChanges();
                return RedirectToAction("Index");  
            }

            ViewBag.PossibleShoppingLists = context.ShoppingLists;
            ViewBag.PossibleItems = context.Items;
            return View(shoppinglistitem);
        }
        
        //
        // GET: /ShoppingListItems/Edit/5
 
        public ActionResult Edit(int id)
        {
            ShoppingListItem shoppinglistitem = context.ShoppingListItems.Single(x => x.Id == id);
            ViewBag.PossibleShoppingLists = context.ShoppingLists;
            ViewBag.PossibleItems = context.Items;
            return View(shoppinglistitem);
        }

        //
        // POST: /ShoppingListItems/Edit/5

        [HttpPost]
        public ActionResult Edit(ShoppingListItem shoppinglistitem)
        {
            if (ModelState.IsValid)
            {
                context.Entry(shoppinglistitem).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PossibleShoppingLists = context.ShoppingLists;
            ViewBag.PossibleItems = context.Items;
            return View(shoppinglistitem);
        }

        //
        // GET: /ShoppingListItems/Delete/5
 
        public ActionResult Delete(int id)
        {
            ShoppingListItem shoppinglistitem = context.ShoppingListItems.Single(x => x.Id == id);
            return View(shoppinglistitem);
        }

        //
        // POST: /ShoppingListItems/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            ShoppingListItem shoppinglistitem = context.ShoppingListItems.Single(x => x.Id == id);
            context.ShoppingListItems.Remove(shoppinglistitem);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult ListOfItems(int id)
        {
            var list = from l in context.ShoppingLists where l.Id == id select l;

            return View(list.Single());
        }

        public ActionResult UpdateRank(int id, int rank)
        {
            ShoppingList list = (from i in context.ShoppingListItems
                                 from l in context.ShoppingLists
                                 where i.Id == id && l.Id == i.ShoppingListId
                                 select l).Single();

            bool foundItem = false;

            List<ShoppingListItem> items = list.ShoppingListItems.OrderBy(i => i.Rank).ToList();

            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].Id == id)
                {
                    items[i].Rank = rank;
                    foundItem = true;
                    continue;
                }

                if (!foundItem && items[i].Rank < rank)
                {
                    // Do nothing.
                }
                else if (!foundItem && items[i].Rank >= rank)
                {
                    items[i].Rank++;
                }
                else if (foundItem && items[i].Rank <= rank)
                {
                    items[i].Rank--;
                }
            }

            context.SaveChanges();

            return View("ListAdd", list);
        }

        public ActionResult Edit(int id, int? rank, double? amount, string unit)
        {
            ShoppingListItem item = (from i in context.ShoppingListItems where i.Id == id select i).Single();

            if (rank.HasValue && amount.HasValue && unit != null)
            {
                item.Rank = rank.Value;
                item.Amount = amount.Value;
                item.Unit = unit;

                context.SaveChanges();

                return View("Details", item);
            }
            else
            {
                return View(item);
            }
        }

        public ActionResult ListAdd(int shoppingListId)
        {
            ShoppingList list = context.ShoppingLists.Where(l => l.Id == shoppingListId).FirstOrDefault();

            return View(list);
        }

        public ActionResult Add(int shoppingListId, string itemString)
        {
            ShoppingListItem item = ItemParser<ShoppingListItem>.Parse(itemString, context);

            ShoppingList list = context.ShoppingLists.Where(l => l.Id == shoppingListId).FirstOrDefault();

            if (list == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                int rank = 0;

                if (list.ShoppingListItems == null)
                {
                    list.ShoppingListItems = new List<ShoppingListItem>();

                    item.Rank = rank;
                }
                else
                {
                    rank = list.ShoppingListItems.Count;
                }

                item.Rank = rank;
                context.ShoppingListItems.Add(item);
                list.ShoppingListItems.Add(item);

                context.SaveChanges();

                return View("ListAdd", list);
            }
        }

        public ActionResult RemoveFromShoppingList(int shoppingListId, int shoppingListItemId)
        {
            ShoppingList list = context.ShoppingLists.Where(l => l.Id == shoppingListId).FirstOrDefault();

            ShoppingListItem item = context.ShoppingListItems.Where(i => i.Id == shoppingListItemId).FirstOrDefault();

            list.ShoppingListItems.Remove(item);

            context.ShoppingListItems.Remove(item);

            context.SaveChanges();

            return RedirectToAction("Details", "ShoppingLists", new { id = list.Id });
        }

    }
}