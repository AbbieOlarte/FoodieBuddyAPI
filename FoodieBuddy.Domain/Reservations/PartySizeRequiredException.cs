using System;

namespace FoodieBuddy.Domain.Reservations
{
    public class PartySizeRequiredException: Exception
    {
        public PartySizeRequiredException(string message): base(message)
        {

        }
    }
}