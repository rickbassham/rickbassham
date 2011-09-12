using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HomeManager.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int CategoryTypeId { get; set; }
        public virtual CategoryType CategoryType { get; set; }

        public virtual ICollection<Recipe> Recipes { get; set; }
    }
}