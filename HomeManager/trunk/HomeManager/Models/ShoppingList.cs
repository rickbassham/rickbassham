using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HomeManager.Models
{
    public class ShoppingList
    {
        public int Id { get; set; }
        public string Store { get; set; }

        public virtual ICollection<ShoppingListItem> ShoppingListItems { get; set; }
    }
}