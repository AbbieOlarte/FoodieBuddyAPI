using System;

namespace FoodieBuddy.Domain.Models.MenuItems
{
    public class MenuItem
    {
        public string FoodName { get; set; }
        public string Ingridients { get; set; }
        public double Price { get; set; }
        public Guid FoodId { get; set; }
    }
}