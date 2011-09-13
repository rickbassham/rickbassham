using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HomeManager.Models
{
    public class CalendarDetailModel
    {
        public DateTime StartDate { get; set; }
        public IDictionary<DateTime, List<Recipe>> ScheduledRecipes { get; set; }
        public List<Recipe> AvailableRecipes { get; set; }
    }
}