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
    public class IngredientsController : Controller
    {
        private HomeManagerContext context = new HomeManagerContext();

        //
        // GET: /Ingredients/

        public ViewResult Index()
        {
            return View(context.Ingredients.Include(ingredient => ingredient.Item).Include(ingredient => ingredient.Recipe).ToList());
        }

        //
        // GET: /Ingredients/Details/5

        public ViewResult Details(int id)
        {
            Ingredient ingredient = context.Ingredients.Single(x => x.Id == id);
            return View(ingredient);
        }

        //
        // GET: /Ingredients/Create

        public ActionResult Create()
        {
            ViewBag.PossibleItems = context.Items;
            ViewBag.PossibleRecipes = context.Recipes;
            return View();
        } 

        //
        // POST: /Ingredients/Create

        [HttpPost]
        public ActionResult Create(Ingredient ingredient)
        {
            if (ModelState.IsValid)
            {
                context.Ingredients.Add(ingredient);
                context.SaveChanges();
                return RedirectToAction("Index");  
            }

            ViewBag.PossibleItems = context.Items;
            ViewBag.PossibleRecipes = context.Recipes;
            return View(ingredient);
        }
        
        //
        // GET: /Ingredients/Edit/5
 
        public ActionResult Edit(int id)
        {
            Ingredient ingredient = context.Ingredients.Single(x => x.Id == id);
            ViewBag.PossibleItems = context.Items;
            ViewBag.PossibleRecipes = context.Recipes;
            return View(ingredient);
        }

        //
        // POST: /Ingredients/Edit/5

        [HttpPost]
        public ActionResult Edit(Ingredient ingredient)
        {
            if (ModelState.IsValid)
            {
                context.Entry(ingredient).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PossibleItems = context.Items;
            ViewBag.PossibleRecipes = context.Recipes;
            return View(ingredient);
        }

        //
        // GET: /Ingredients/Delete/5
 
        public ActionResult Delete(int id)
        {
            Ingredient ingredient = context.Ingredients.Single(x => x.Id == id);
            return View(ingredient);
        }

        //
        // POST: /Ingredients/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Ingredient ingredient = context.Ingredients.Single(x => x.Id == id);
            context.Ingredients.Remove(ingredient);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult RemoveFromRecipe(int recipeId, int ingredientId)
        {
            Recipe r = (from i in context.Recipes where i.Id == recipeId select i).Single();

            Ingredient ingredient = (from i in context.Ingredients where i.Id == ingredientId select i).Single();

            r.Ingredients.Remove(ingredient);

            context.Ingredients.Remove(ingredient);

            context.SaveChanges();

            return RedirectToAction("Details", "Recipes", new { id = recipeId });
        }

        public ActionResult GetUnits(string term)
        {
            var units = (from u in context.Ingredients where u.Unit.StartsWith(term) select u.Unit).Distinct();

            return Json(units.ToList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Add(int recipeId, string ingredient)
        {
            Ingredient parsedIngredient = ItemParser<Ingredient>.Parse(ingredient, context);

            Recipe r = (from i in context.Recipes where i.Id == recipeId select i).Single();

            r.Ingredients.Add(parsedIngredient);

            context.SaveChanges();

            return RedirectToAction("Details", "Recipes", new { id = r.Id });
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
    }
}