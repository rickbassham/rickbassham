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
    public class RecipeStepsController : Controller
    {
        private HomeManagerContext context = new HomeManagerContext();

        //
        // GET: /RecipeSteps/

        public ViewResult Index()
        {
            return View(context.RecipeSteps.Include(recipestep => recipestep.Recipe).ToList());
        }

        //
        // GET: /RecipeSteps/Details/5

        public ViewResult Details(int id)
        {
            RecipeStep recipestep = context.RecipeSteps.Single(x => x.Id == id);
            return View(recipestep);
        }

        //
        // GET: /RecipeSteps/Create

        public ActionResult Create()
        {
            ViewBag.PossibleRecipes = context.Recipes;
            return View();
        } 

        //
        // POST: /RecipeSteps/Create

        [HttpPost]
        public ActionResult Create(RecipeStep recipestep)
        {
            if (ModelState.IsValid)
            {
                context.RecipeSteps.Add(recipestep);
                context.SaveChanges();
                return RedirectToAction("Index");  
            }

            ViewBag.PossibleRecipes = context.Recipes;
            return View(recipestep);
        }
        
        //
        // GET: /RecipeSteps/Edit/5
 
        public ActionResult Edit(int id)
        {
            RecipeStep recipestep = context.RecipeSteps.Single(x => x.Id == id);
            ViewBag.PossibleRecipes = context.Recipes;
            return View(recipestep);
        }

        //
        // POST: /RecipeSteps/Edit/5

        [HttpPost]
        public ActionResult Edit(RecipeStep recipestep)
        {
            if (ModelState.IsValid)
            {
                context.Entry(recipestep).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PossibleRecipes = context.Recipes;
            return View(recipestep);
        }

        //
        // GET: /RecipeSteps/Delete/5
 
        public ActionResult Delete(int id)
        {
            RecipeStep recipestep = context.RecipeSteps.Single(x => x.Id == id);
            return View(recipestep);
        }

        //
        // POST: /RecipeSteps/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            RecipeStep recipestep = context.RecipeSteps.Single(x => x.Id == id);
            context.RecipeSteps.Remove(recipestep);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult RemoveFromRecipe(int recipeId, int stepId)
        {
            Recipe r = (from i in context.Recipes where i.Id == recipeId select i).Single();

            RecipeStep step = (from i in context.RecipeSteps where i.Id == stepId select i).Single();

            r.RecipeSteps.Remove(step);

            context.RecipeSteps.Remove(step);

            context.SaveChanges();

            return View("ListAdd", r);
        }

        public ActionResult ListAdd(int recipeId)
        {
            Recipe r = (from i in context.Recipes where i.Id == recipeId select i).Single();

            return View(r);
        }

        public ActionResult List(int recipeId)
        {
            Recipe r = (from i in context.Recipes where i.Id == recipeId select i).Single();

            return View(r);
        }

        public ActionResult Add(int recipeId, string stepDescription)
        {
            Recipe r = (from i in context.Recipes where i.Id == recipeId select i).Single();

            RecipeStep step = new RecipeStep();

            if (r.RecipeSteps.Count > 0)
            {
                step.Rank = (short)(r.RecipeSteps.Max(s => s.Rank) + 1);
            }
            else
            {
                step.Rank = 0;
            }

            step.Description = stepDescription;

//            c.RecipeSteps.AddObject(step);

            r.RecipeSteps.Add(step);
            context.SaveChanges();

            return View("ListAdd", r);
        }
    }
}