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
    public class RecipeNotesController : Controller
    {
        private HomeManagerContext context = new HomeManagerContext();

        //
        // GET: /RecipeNotes/

        public ViewResult Index()
        {
            return View(context.RecipeNotes.Include(recipenote => recipenote.Recipe).ToList());
        }

        //
        // GET: /RecipeNotes/Details/5

        public ViewResult Details(int id)
        {
            RecipeNote recipenote = context.RecipeNotes.Single(x => x.Id == id);
            return View(recipenote);
        }

        //
        // GET: /RecipeNotes/Create

        public ActionResult Create()
        {
            ViewBag.PossibleRecipes = context.Recipes;
            return View();
        } 

        //
        // POST: /RecipeNotes/Create

        [HttpPost]
        public ActionResult Create(RecipeNote recipenote)
        {
            if (ModelState.IsValid)
            {
                context.RecipeNotes.Add(recipenote);
                context.SaveChanges();
                return RedirectToAction("Index");  
            }

            ViewBag.PossibleRecipes = context.Recipes;
            return View(recipenote);
        }
        
        //
        // GET: /RecipeNotes/Edit/5
 
        public ActionResult Edit(int id)
        {
            RecipeNote recipenote = context.RecipeNotes.Single(x => x.Id == id);
            ViewBag.PossibleRecipes = context.Recipes;
            return View(recipenote);
        }

        //
        // POST: /RecipeNotes/Edit/5

        [HttpPost]
        public ActionResult Edit(RecipeNote recipenote)
        {
            if (ModelState.IsValid)
            {
                context.Entry(recipenote).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PossibleRecipes = context.Recipes;
            return View(recipenote);
        }

        //
        // GET: /RecipeNotes/Delete/5
 
        public ActionResult Delete(int id)
        {
            RecipeNote recipenote = context.RecipeNotes.Single(x => x.Id == id);
            return View(recipenote);
        }

        //
        // POST: /RecipeNotes/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            RecipeNote recipenote = context.RecipeNotes.Single(x => x.Id == id);
            context.RecipeNotes.Remove(recipenote);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}