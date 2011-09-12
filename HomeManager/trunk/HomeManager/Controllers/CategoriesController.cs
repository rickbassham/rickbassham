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
    public class CategoriesController : Controller
    {
        private HomeManagerContext context = new HomeManagerContext();

        //
        // GET: /Categories/

        public ViewResult Index()
        {
            return View(context.Categories.Include(category => category.CategoryType).Include(category => category.Recipes).ToList());
        }

        //
        // GET: /Categories/Details/5

        public ViewResult Details(int id)
        {
            Category category = context.Categories.Single(x => x.Id == id);
            return View(category);
        }

        //
        // GET: /Categories/Create

        public ActionResult Create()
        {
            ViewBag.PossibleCategoryTypes = context.CategoryTypes;
            return View();
        } 

        //
        // POST: /Categories/Create

        [HttpPost]
        public ActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                context.Categories.Add(category);
                context.SaveChanges();
                return RedirectToAction("Index");  
            }

            ViewBag.PossibleCategoryTypes = context.CategoryTypes;
            return View(category);
        }
        
        //
        // GET: /Categories/Edit/5
 
        public ActionResult Edit(int id)
        {
            Category category = context.Categories.Single(x => x.Id == id);
            ViewBag.PossibleCategoryTypes = context.CategoryTypes;
            return View(category);
        }

        //
        // POST: /Categories/Edit/5

        [HttpPost]
        public ActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                context.Entry(category).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PossibleCategoryTypes = context.CategoryTypes;
            return View(category);
        }

        //
        // GET: /Categories/Delete/5
 
        public ActionResult Delete(int id)
        {
            Category category = context.Categories.Single(x => x.Id == id);
            return View(category);
        }

        //
        // POST: /Categories/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = context.Categories.Single(x => x.Id == id);
            context.Categories.Remove(category);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}