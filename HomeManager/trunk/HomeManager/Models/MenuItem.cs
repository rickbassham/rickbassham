﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HomeManager.Models
{
    public class MenuItem
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Meal { get; set; }

        public virtual ICollection<Recipe> Recipes { get; set; }
    }
}