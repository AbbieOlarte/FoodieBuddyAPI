using System;

namespace FoodieBuddy.Domain.MailingList
{
    public class EmailRequiredException: Exception
    {
        public EmailRequiredException(string message): base(message)
        {

        }
    }
}