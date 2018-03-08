using FoodieBuddy.Domain.Models.MailingList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodieBuddy.API.Utils
{
    public static class MailExtensions
    {
        public static Mail ApplyChanges(this Mail mail, Mail from)
        {
            mail.Email = from.Email;

            return mail;
        }
    }
}
