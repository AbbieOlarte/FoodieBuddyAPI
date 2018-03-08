using System;

namespace FoodieBuddy.Domain.MenuItems
{
    public class FoodNameRequiredException: Exception
    {
        public FoodNameRequiredException(string message): base(message)
        {
         
        }
    }
}