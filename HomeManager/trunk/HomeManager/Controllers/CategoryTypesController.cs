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
    public class CategoryTypesController : Controller
    {
        private HomeManagerContext context = new HomeManagerContext();

        //
        // GET: /CategoryTypes/

        public ViewResult Index()
        {
            return View(context.CategoryTypes.Include(categorytype => categorytype.Categories).ToList());
        }

        //
        // GET: /CategoryTypes/Details/5

        public ViewResult Details(int id)
        {
            CategoryType categorytype = context.CategoryTypes.Single(x => x.Id == id);
            return View(categorytype);
        }

        //
        // GET: /CategoryTypes/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /CategoryTypes/Create

        [HttpPost]
        public ActionResult Create(CategoryType categorytype)
        {
            if (ModelState.IsValid)
            {
                context.CategoryTypes.Add(categorytype);
                context.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(categorytype);
        }
        
        //
        // GET: /CategoryTypes/Edit/5
 
        public ActionResult Edit(int id)
        {
            CategoryType categorytype = context.CategoryTypes.Single(x => x.Id == id);
            return View(categorytype);
        }

        //
        // POST: /CategoryTypes/Edit/5

        [HttpPost]
        public ActionResult Edit(CategoryType categorytype)
        {
            if (ModelState.IsValid)
            {
                context.Entry(categorytype).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(categorytype);
        }

        //
        // GET: /CategoryTypes/Delete/5
 
        public ActionResult Delete(int id)
        {
            CategoryType categorytype = context.CategoryTypes.Single(x => x.Id == id);
            return View(categorytype);
        }

        //
        // POST: /CategoryTypes/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            CategoryType categorytype = context.CategoryTypes.Single(x => x.Id == id);
            context.CategoryTypes.Remove(categorytype);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}