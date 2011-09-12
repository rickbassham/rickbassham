using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HomeManager.Models
{
    public class ShoppingListItem : IItem
    {
        public int Id { get; set; }
        public double Amount { get; set; }
        public string Unit { get; set; }
        public int Rank { get; set; }

        public int ShoppingListId { get; set; }
        public virtual ShoppingList ShoppingList { get; set; }

        public int ItemId { get; set; }
        public virtual Item Item { get; set; }
    }
}