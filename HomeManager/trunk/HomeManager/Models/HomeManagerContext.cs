using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace HomeManager.Models
{
    public class HomeManagerContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, add the following
        // code to the Application_Start method in your Global.asax file.
        // Note: this will destroy and re-create your database with every model change.
        // 
        // System.Data.Entity.Database.SetInitializer(new System.Data.Entity.DropCreateDatabaseIfModelChanges<HomeManager.Models.HomeManagerContext>());

        public DbSet<HomeManager.Models.Category> Categories { get; set; }

        public DbSet<HomeManager.Models.CategoryType> CategoryTypes { get; set; }

        public DbSet<HomeManager.Models.Ingredient> Ingredients { get; set; }

        public DbSet<HomeManager.Models.Item> Items { get; set; }

        public DbSet<HomeManager.Models.ItemType> ItemTypes { get; set; }

        public DbSet<HomeManager.Models.MenuItem> MenuItems { get; set; }

        public DbSet<HomeManager.Models.Recipe> Recipes { get; set; }

        public DbSet<HomeManager.Models.RecipeNote> RecipeNotes { get; set; }

        public DbSet<HomeManager.Models.RecipeStep> RecipeSteps { get; set; }

        public DbSet<HomeManager.Models.ShoppingList> ShoppingLists { get; set; }

        public DbSet<HomeManager.Models.ShoppingListItem> ShoppingListItems { get; set; }
    }
}