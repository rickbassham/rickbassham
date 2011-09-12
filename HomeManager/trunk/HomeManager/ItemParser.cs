using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HomeManager.Models;

namespace HomeManager
{
    public class ItemParser<TModel> where TModel : IItem, new()
    {
        private static string[] UNITS = new string[] {
            "cup",
            "cups",
            "teaspoon",
            "teaspoons",
            "tablespoon",
            "tablespoons",
            "ounce",
            "ounces",
        };

        public static TModel Parse(string str, HomeManagerContext c)
        {
            Mehroz.Fraction amount = 0;

            int i = 1;

            while (true)
            {
                try
                {
                    if (str[i - 1] == ' ' || str[i - 1] == '/')
                    {
                        i++;
                    }

                    amount = Mehroz.Fraction.ToFraction(str.Substring(0, i++));
                }
                catch
                {
                    break;
                }
            }

            TModel item = new TModel();

            item.Amount = amount.ToDouble();

            string[] unitItem = str.Substring(i - 2).Split(' ');

            string unit = string.Empty;
            string itemName = string.Empty;

            if (unitItem.Length > 1 && UNITS.Contains(unitItem[0]))
            {
                unit = unitItem[0];
                itemName = string.Join(" ", unitItem, 1, unitItem.Length - 1);
            }
            else
            {
                unit = string.Empty;
                itemName = str.Substring(i - 2);
            }

            item.Unit = unit;
            item.Item = (from it in c.Items where it.Name == itemName select it).SingleOrDefault();

            if (item.Item == null)
            {
                item.Item = new Item();
                item.Item.Name = itemName;
                item.Item.ItemType = (from itemType in c.ItemTypes where itemType.Name == "Recipe" select itemType).FirstOrDefault();

                if (item.Item.ItemType == null)
                {
                    item.Item.ItemType = new ItemType { Name = "Recipe" };
                    c.ItemTypes.Add(item.Item.ItemType);
                }
            }

            return item;
        }

    }
}