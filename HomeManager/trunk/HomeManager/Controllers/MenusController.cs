using System;
using System.Collections.Generic;
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
    public class MenusController : Controller
    {
        private HomeManagerContext context = new HomeManagerContext();

        public ActionResult Details()
        {
            var list = (from r in context.Recipes select r);

            return View(list.ToList());
        }

        public ActionResult RemoveRecipe(int recipeId, long unixDate)
        {
            DateTime date = unixDate.FromUnixTimestamp();

            MenuItem menuItem = (from mi in context.MenuItems where mi.Date == date.Date select mi).Single();

            Recipe r = (from recipe in context.Recipes where recipe.Id == recipeId select recipe).Single();

            menuItem.Recipes.Remove(r);

            if (menuItem.Recipes.Count == 0)
            {
                context.MenuItems.Remove(menuItem);
            }

            context.SaveChanges();

            return Json("", JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddRecipe(int recipeId, long unixDate)
        {
            DateTime date = unixDate.FromUnixTimestamp();

            var item = from mi in context.MenuItems where mi.Date == date.Date select mi;

            MenuItem menuItem;

            if (item.Count() > 0)
            {
                menuItem = item.Single();
            }
            else
            {
                menuItem = new MenuItem();

                menuItem.Date = date.Date;
                menuItem.Meal = "Dinner";

                context.MenuItems.Add(menuItem);
            }

            Recipe r = (from recipe in context.Recipes where recipe.Id == recipeId select recipe).Single();

            if (menuItem.Recipes == null)
            {
                menuItem.Recipes = new List<Recipe>();
            }

            menuItem.Recipes.Add(r);

            context.SaveChanges();

            return Json("", JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetMeals(double start, double end)
        {
            DateTime startDate = start.FromUnixTimestamp().Date;
            DateTime endDate = end.FromUnixTimestamp().Date;

            var menuItems = from derivedItems in
                                (from recipe in context.Recipes
                                 from item in recipe.MenuItems
                                 where item.Date >= startDate && item.Date <= endDate
                                 select new
                                 {
                                     id = recipe.Id,
                                     title = recipe.Name,
                                     allDay = true,
                                     start = item.Date
                                 }).ToList()
                            select new
                            {
                                id = derivedItems.id,
                                title = derivedItems.title,
                                allDay = derivedItems.allDay,
                                start = derivedItems.start.ToString("s")
                            };

            return Json(menuItems, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Calendar()
        {
            iCalendar cal = new iCalendar();

            var items = context.MenuItems.OrderBy(m => m.Date);

            foreach (var item in items)
            {
                int offset = 0;

                switch (item.Meal)
                {
                    case "Dinner": offset = 18; break;
                    case "Lunch": offset = 12; break;
                    case "Breakfast": offset = 8; break;
                }

                foreach (var recipe in item.Recipes)
                {
                    string path = RouteTable.Routes.GetVirtualPath(
                        new RequestContext(HttpContext, RouteTable.Routes.GetRouteData(HttpContext)),
                        new RouteValueDictionary(new
                        {
                            Controller = "Recipe",
                            Action = "Details",
                            Id = recipe.Id
                        })).VirtualPath;

                    cal.AddChild(new Event
                    {
                        Start = new iCalDateTime(item.Date.Date.AddHours(offset)),
                        End = new iCalDateTime(item.Date.Date.AddHours(offset + 1)),
                        Name = recipe.Name,
                        Summary = recipe.Description,
                        Url = new Uri(string.Format("http://{0}/{1}", this.HttpContext.Request.Url.Host, path))
                    });
                }
            }

            iCalendarSerializer serializer = new iCalendarSerializer();

            string response = serializer.SerializeToString(cal);

            return new ContentResult
            {
                Content = response,
                ContentType = "text/calendar",
                ContentEncoding = Encoding.UTF8
            };
        }
    }
}
