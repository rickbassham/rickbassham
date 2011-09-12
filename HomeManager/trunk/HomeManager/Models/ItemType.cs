using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HomeManager.Models
{
    public class ItemType
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Item> Items { get; set; }
    }
}