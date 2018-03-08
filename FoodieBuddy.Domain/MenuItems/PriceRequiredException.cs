using System;

namespace FoodieBuddy.Domain.MenuItems
{
    public class PriceRequiredException: Exception
    {
        public PriceRequiredException(string message): base(message)
        {

        }
    }
}