using System;

namespace FoodieBuddy.Domain.Models.MailingList
{
    public class Mail
    {
        public string Email { get; set; }
        public Guid MailingId { get; set; }
    }
}