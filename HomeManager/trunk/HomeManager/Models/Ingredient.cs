using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HomeManager.Models
{
    public class Ingredient : IItem
    {
        public int Id { get; set; }
        public double Amount { get; set; }
        public string Unit { get; set; }

        public int ItemId { get; set; }
        public virtual Item Item { get; set; }

        public int RecipeId { get; set; }
        public virtual Recipe Recipe { get; set; }

    }
}