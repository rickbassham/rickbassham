using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HomeManager.Models;
using DDay.iCal;
using System.Web.Routing;
using DDay.iCal.Serialization.iCalendar;
using System.Text;

namespace HomeManager.Controllers
{   
    public class MenuItemsController : Controller
    {
        private HomeManagerContext context = new HomeManagerContext();

        //
        // GET: /MenuItems/

        public ViewResult Index()
        {
            return View(context.MenuItems.Include(menuitem => menuitem.Recipes).ToList());
        }

        //
        // GET: /MenuItems/Details/5

        public ViewResult Details(int id)
        {
            MenuItem menuitem = context.MenuItems.Single(x => x.Id == id);
            return View(menuitem);
        }

        //
        // GET: /MenuItems/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /MenuItems/Create

        [HttpPost]
        public ActionResult Create(MenuItem menuitem)
        {
            if (ModelState.IsValid)
            {
                context.MenuItems.Add(menuitem);
                context.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(menuitem);
        }
        
        //
        // GET: /MenuItems/Edit/5
 
        public ActionResult Edit(int id)
        {
            MenuItem menuitem = context.MenuItems.Single(x => x.Id == id);
            return View(menuitem);
        }

        //
        // POST: /MenuItems/Edit/5

        [HttpPost]
        public ActionResult Edit(MenuItem menuitem)
        {
            if (ModelState.IsValid)
            {
                context.Entry(menuitem).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(menuitem);
        }

        //
        // GET: /MenuItems/Delete/5
 
        public ActionResult Delete(int id)
        {
            MenuItem menuitem = context.MenuItems.Single(x => x.Id == id);
            return View(menuitem);
        }

        //
        // POST: /MenuItems/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            MenuItem menuitem = context.MenuItems.Single(x => x.Id == id);
            context.MenuItems.Remove(menuitem);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}