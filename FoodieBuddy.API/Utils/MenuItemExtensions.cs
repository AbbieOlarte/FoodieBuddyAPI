using FoodieBuddy.Domain.Models.MenuItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodieBuddy.API.Utils
{
    public static class MenuItemExtensions
    {
        public static MenuItem ApplyChanges(this MenuItem foodItem, MenuItem from)
        {
            foodItem.FoodName = from.FoodName;
            foodItem.Ingridients = from.Ingridients;
            foodItem.Price = from.Price;

            return foodItem;
        }
    }
}
