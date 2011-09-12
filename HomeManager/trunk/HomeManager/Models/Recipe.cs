using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HomeManager.Models
{
    public class Recipe
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Rating { get; set; }

        public long PrepTimeBacking { get; set; }
        public long InactivePrepTimeBacking { get; set; }
        public long CookTimeBacking { get; set; }

        public string Source { get; set; }
        public int Servings { get; set; }

        public virtual ICollection<Category> Categories { get; set; }
        public virtual ICollection<RecipeStep> RecipeSteps { get; set; }
        public virtual ICollection<RecipeNote> RecipeNotes { get; set; }
        public virtual ICollection<MenuItem> MenuItems { get; set; }
        public virtual ICollection<Ingredient> Ingredients { get; set; }

        [NotMapped]
        public TimeSpan PrepTime
        {
            get
            {
                return TimeSpan.FromTicks(PrepTimeBacking);
            }
            set
            {
                PrepTimeBacking = value.Ticks;
            }
        }


        [NotMapped]
        public TimeSpan InactivePrepTime
        {
            get
            {
                return TimeSpan.FromTicks(InactivePrepTimeBacking);
            }
            set
            {
                InactivePrepTimeBacking = value.Ticks;
            }
        }

        [NotMapped]
        public TimeSpan CookTime
        {
            get
            {
                return TimeSpan.FromTicks(CookTimeBacking);
            }
            set
            {
                CookTimeBacking = value.Ticks;
            }
        }

    }
}