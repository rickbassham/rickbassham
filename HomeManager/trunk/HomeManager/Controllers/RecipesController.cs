using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HomeManager.Models;
using System.IO;
using System.Net;
using HtmlAgilityPack;
using System.Text;

namespace HomeManager.Controllers
{   
    public class RecipesController : Controller
    {
        private HomeManagerContext context = new HomeManagerContext();

        //
        // GET: /Recipes/

        public ViewResult Index()
        {
            return View(context.Recipes.Include(recipe => recipe.Categories).Include(recipe => recipe.RecipeSteps).Include(recipe => recipe.RecipeNotes).Include(recipe => recipe.MenuItems).Include(recipe => recipe.Ingredients).ToList());
        }

        //
        // GET: /Recipes/Details/5

        public ViewResult Details(int id)
        {
            Recipe recipe = context.Recipes.Single(x => x.Id == id);
            return View(recipe);
        }

        //
        // GET: /Recipes/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // GET: /Recipes/Edit/5
 
        public ActionResult Edit(int id)
        {
            Recipe recipe = context.Recipes.Single(x => x.Id == id);
            return View(recipe);
        }

        //
        // POST: /Recipes/Edit/5

        [HttpPost]
        public ActionResult Edit(Recipe recipe)
        {
            if (ModelState.IsValid)
            {
                context.Entry(recipe).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(recipe);
        }

        //
        // GET: /Recipes/Delete/5
 
        public ActionResult Delete(int id)
        {
            Recipe recipe = context.Recipes.Single(x => x.Id == id);
            return View(recipe);
        }

        //
        // POST: /Recipes/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Recipe recipe = context.Recipes.Single(x => x.Id == id);
            context.Recipes.Remove(recipe);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        private Stream GetWebStream(string url)
        {
            using (WebClient client = new WebClient())
            {
                return client.OpenRead(url);
            }
        }

        public ActionResult AddFromCookingChannel(string url)
        {
            HtmlDocument doc = new HtmlDocument();

            doc.Load(GetWebStream(url));

            Recipe r = new Recipe();

            r.Name = doc.DocumentNode.SelectSingleNode(@"//h1/span").InnerText.Trim();
            r.Description = string.Empty;
            r.Source = url;

            r.Ingredients = new List<Ingredient>();

            foreach (HtmlNode ingredientNode in doc.DocumentNode.SelectNodes(@"//li[@class=""ingredient""]"))
            {
                Ingredient i = ItemParser<Ingredient>.Parse(ingredientNode.InnerText.Trim(), context);

                context.Ingredients.Add(i);

                r.Ingredients.Add(i);
            }

            r.RecipeSteps = new List<RecipeStep>();

            foreach (HtmlNode directionNode in doc.DocumentNode.SelectNodes(@"//div[@class=""instructions""]"))
            {
                string directions = directionNode.InnerHtml;

                string[] directionList = directions.Split(new string[] { "<br>" }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string dir in directionList)
                {
                    if (dir.Trim().Length == 0)
                    {
                        continue;
                    }

                    RecipeStep rs = new RecipeStep();

                    rs.Description = dir.Trim();

                    context.RecipeSteps.Add(rs);

                    r.RecipeSteps.Add(rs);
                }
            }

            context.Recipes.Add(r);

            context.SaveChanges();

            return RedirectToAction("Details", new { id = r.Id });

        }

        private TimeSpan ParseAllRecipesTime(string timeCode)
        {
            if (timeCode.StartsWith("PT"))
            {
                timeCode = timeCode.Substring(2);

                StringBuilder number = new StringBuilder();

                int hours = 0;
                int minutes = 0;

                for (int i = 0; i < timeCode.Length; i++)
                {
                    if (timeCode[i] >= 48 && timeCode[i] <= 57)
                    {
                        number.Append(timeCode[i]);
                    }
                    else
                    {
                        switch(timeCode[i])
                        {
                            case 'H': hours = int.Parse(number.ToString()); break;
                            case 'M': minutes = int.Parse(number.ToString()); break;
                        }

                        number.Clear();
                    }
                }

                return new TimeSpan(hours, minutes, 0);
            }
            else
            {
                return TimeSpan.Zero;
            }
        }

        public ActionResult AddFromAllrecipes(string url)
        {
            HtmlDocument doc = new HtmlDocument();

            doc.Load(GetWebStream(url));

            Recipe r = new Recipe();

            r.Name = doc.DocumentNode.SelectSingleNode(@"//h1[@id=""itemTitle""]/span").InnerText.Trim();

            r.Description = doc.DocumentNode.SelectSingleNode(@"//div[@class=""rec-title_submitterdiv""]/div/div/div[@class=""plaincharacterwrap""]").InnerText.Trim();

            string prepTimeCode = doc.DocumentNode.SelectSingleNode(@"//span[@class=""prepTime""]/span").Attributes["title"].Value;
            string cookTimeCode = doc.DocumentNode.SelectSingleNode(@"//span[@class=""cookTime""]/span").Attributes["title"].Value;
            string totalTimeCode = doc.DocumentNode.SelectSingleNode(@"//span[@class=""totalTime""]/span").Attributes["title"].Value;

            r.PrepTime = ParseAllRecipesTime(prepTimeCode);
            r.CookTime = ParseAllRecipesTime(cookTimeCode);
            r.InactivePrepTime = ParseAllRecipesTime(totalTimeCode) - r.PrepTime - r.CookTime;

            r.Servings = int.Parse(doc.DocumentNode.SelectSingleNode(@"//input[@class=""servings-amount""]").Attributes["value"].Value);

            r.Source = url;

            r.Ingredients = new List<Ingredient>();

            foreach (HtmlNode ingredientNode in doc.DocumentNode.SelectNodes(@"//div[@class=""ingredients""]/ul/li"))
            {
                Ingredient i = ItemParser<Ingredient>.Parse(ingredientNode.InnerText.Trim(), context);

                context.Ingredients.Add(i);

                r.Ingredients.Add(i);
            }

            r.RecipeSteps = new List<RecipeStep>();

            foreach (HtmlNode directionNode in doc.DocumentNode.SelectNodes(@"//div[@class=""directions""]/ol/li"))
            {
                RecipeStep rs = new RecipeStep();

                rs.Description = directionNode.InnerText.Trim();

                context.RecipeSteps.Add(rs);

                r.RecipeSteps.Add(rs);
            }

            context.Recipes.Add(r);

            context.SaveChanges();

            return RedirectToAction("Details", new { id = r.Id });
        }

        [ChildActionOnly]
        public ActionResult List()
        {
            var list = (from r in context.Recipes select r);

            return View(list.ToList());
        }

        [HttpPost]
        public ActionResult Create(string Name, string Description, string CookTime, string PrepTime, string Source, short? Rating, short? Servings)
        {
            if (string.IsNullOrWhiteSpace(Name) && Source.StartsWith("http://allrecipes.com"))
            {
                return RedirectToAction("AddFromAllrecipes", new { url = Source });
            }

            if (string.IsNullOrWhiteSpace(Name) && Source.StartsWith("http://www.cookingchanneltv.com"))
            {
                return RedirectToAction("AddFromCookingChannel", new { url = Source });
            }

            try
            {
                Recipe r = new Recipe();

                r.Name = Name;
                r.Description = Description;
                r.CookTime = TimeSpan.Parse(CookTime);
                r.PrepTime = TimeSpan.Parse(PrepTime);
                r.Rating = Rating.Value;
                r.Source = Source;
                r.Servings = Servings.Value;

                context.Recipes.Add(r);

                context.SaveChanges();

                return RedirectToAction("Details", new { id = r.Id });
            }
            catch
            {
                return View();
            }
        }
    }
}