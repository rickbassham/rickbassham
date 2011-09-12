using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HomeManager.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int ItemTypeId { get; set; }
        public virtual ItemType ItemType { get; set; }

        public virtual ICollection<Ingredient> Ingredients { get; set; }
        public virtual ICollection<ShoppingListItem> ShoppingListItems { get; set; }
    }
}