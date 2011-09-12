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
    public class ItemsController : Controller
    {
        private HomeManagerContext context = new HomeManagerContext();

        //
        // GET: /Items/

        public ViewResult Index()
        {
            return View(context.Items.Include(item => item.ItemType).Include(item => item.Ingredients).Include(item => item.ShoppingListItems).ToList());
        }

        //
        // GET: /Items/Details/5

        public ViewResult Details(int id)
        {
            Item item = context.Items.Single(x => x.Id == id);
            return View(item);
        }

        //
        // GET: /Items/Create

        public ActionResult Create()
        {
            ViewBag.PossibleItemTypes = context.ItemTypes;
            return View();
        } 

        //
        // POST: /Items/Create

        [HttpPost]
        public ActionResult Create(Item item)
        {
            if (ModelState.IsValid)
            {
                context.Items.Add(item);
                context.SaveChanges();
                return RedirectToAction("Index");  
            }

            ViewBag.PossibleItemTypes = context.ItemTypes;
            return View(item);
        }
        
        //
        // GET: /Items/Edit/5
 
        public ActionResult Edit(int id)
        {
            Item item = context.Items.Single(x => x.Id == id);
            ViewBag.PossibleItemTypes = context.ItemTypes;
            return View(item);
        }

        //
        // POST: /Items/Edit/5

        [HttpPost]
        public ActionResult Edit(Item item)
        {
            if (ModelState.IsValid)
            {
                context.Entry(item).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PossibleItemTypes = context.ItemTypes;
            return View(item);
        }

        //
        // GET: /Items/Delete/5
 
        public ActionResult Delete(int id)
        {
            Item item = context.Items.Single(x => x.Id == id);
            return View(item);
        }

        //
        // POST: /Items/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Item item = context.Items.Single(x => x.Id == id);
            context.Items.Remove(item);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult GetItems(string term)
        {
            var items = (from u in context.Items where u.Name.StartsWith(term) select u.Name).Distinct();

            return Json(items.ToList(), JsonRequestBehavior.AllowGet);
        }

    }
}