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
using System.Data.Objects;

namespace HomeManager.Controllers
{
    public class MenusController : Controller
    {
        private HomeManagerContext context = new HomeManagerContext();

        [HttpGet]
        public ActionResult Details(DateTime? date)
        {
            DateTime startDate = (date ?? DateTime.Now).Date;
            startDate = startDate.AddDays(-1 * (int)startDate.DayOfWeek);

            DateTime endDate = startDate.AddDays(7);

            var scheduledRecipes = (from menuItem in context.MenuItems
                                    group menuItem by menuItem.Date into m
                                    where m.Key >= startDate && m.Key < endDate
                                    select m).ToDictionary(group => group.Key, group => group.Select(item => item.Recipe).ToList());

            CalendarDetailModel retVal = new CalendarDetailModel();

            retVal.StartDate = startDate;
            retVal.ScheduledRecipes = scheduledRecipes;
            retVal.AvailableRecipes = context.Recipes.ToList();

            retVal.AvailableRecipes.Add(new Recipe { Id = 0, Name = "Select Recipe" });

            retVal.AvailableRecipes.Sort((a, b) => a.Id.CompareTo(b.Id));

            return View(retVal);
        }

        [HttpPost]
        public ActionResult Details(DateTime date, int recipeId)
        {
            date = date.Date;

            var recipe = context.Recipes.Where(r => r.Id == recipeId).FirstOrDefault();

            if (recipe != null)
            {
                MenuItem item = new MenuItem
                {
                    Date = date,
                    Meal = "Dinner",
                    RecipeId = recipeId
                };

                context.MenuItems.Add(item);
                context.SaveChanges();
            }

            return Details(date);
        }

        public ActionResult RemoveRecipe(DateTime date, int recipeId)
        {
            date = date.Date;

            var recipe = context.Recipes.Where(r => r.Id == recipeId).FirstOrDefault();

            if (recipe != null)
            {
                MenuItem item = context.MenuItems.Where(m => m.RecipeId == recipeId && m.Date == date).FirstOrDefault();

                context.MenuItems.Remove(item);
                context.SaveChanges();
            }

            return RedirectToAction("Details", new { date = date });
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

                string path = RouteTable.Routes.GetVirtualPath(
                    new RequestContext(HttpContext, RouteTable.Routes.GetRouteData(HttpContext)),
                    new RouteValueDictionary(new
                    {
                        Controller = "Recipe",
                        Action = "Details",
                        Id = item.Recipe.Id
                    })).VirtualPath;

                cal.AddChild(new Event
                {
                    Start = new iCalDateTime(item.Date.Date.AddHours(offset)),
                    End = new iCalDateTime(item.Date.Date.AddHours(offset + 1)),
                    Name = item.Recipe.Name,
                    Summary = item.Recipe.Description,
                    Url = new Uri(string.Format("http://{0}/{1}", this.HttpContext.Request.Url.Host, path))
                });
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
