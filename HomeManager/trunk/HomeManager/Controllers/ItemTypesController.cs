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
    public class ItemTypesController : Controller
    {
        private HomeManagerContext context = new HomeManagerContext();

        //
        // GET: /ItemTypes/

        public ViewResult Index()
        {
            return View(context.ItemTypes.Include(itemtype => itemtype.Items).ToList());
        }

        //
        // GET: /ItemTypes/Details/5

        public ViewResult Details(int id)
        {
            ItemType itemtype = context.ItemTypes.Single(x => x.Id == id);
            return View(itemtype);
        }

        //
        // GET: /ItemTypes/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /ItemTypes/Create

        [HttpPost]
        public ActionResult Create(ItemType itemtype)
        {
            if (ModelState.IsValid)
            {
                context.ItemTypes.Add(itemtype);
                context.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(itemtype);
        }
        
        //
        // GET: /ItemTypes/Edit/5
 
        public ActionResult Edit(int id)
        {
            ItemType itemtype = context.ItemTypes.Single(x => x.Id == id);
            return View(itemtype);
        }

        //
        // POST: /ItemTypes/Edit/5

        [HttpPost]
        public ActionResult Edit(ItemType itemtype)
        {
            if (ModelState.IsValid)
            {
                context.Entry(itemtype).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(itemtype);
        }

        //
        // GET: /ItemTypes/Delete/5
 
        public ActionResult Delete(int id)
        {
            ItemType itemtype = context.ItemTypes.Single(x => x.Id == id);
            return View(itemtype);
        }

        //
        // POST: /ItemTypes/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            ItemType itemtype = context.ItemTypes.Single(x => x.Id == id);
            context.ItemTypes.Remove(itemtype);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}