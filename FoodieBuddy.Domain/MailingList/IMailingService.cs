using FoodieBuddy.Domain.Models.MailingList;
using System;

namespace FoodieBuddy.Domain.MailingList
{
    public interface IMailingService
    {
        Mail Save(Guid id, Mail email);
    }
}