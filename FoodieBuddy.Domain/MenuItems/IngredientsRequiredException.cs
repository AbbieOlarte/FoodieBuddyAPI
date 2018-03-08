using System;

namespace FoodieBuddy.Domain.MenuItems
{
    public class IngredientsRequiredException: Exception
    {
        public IngredientsRequiredException(string message): base(message)
        {

        }
    }
}