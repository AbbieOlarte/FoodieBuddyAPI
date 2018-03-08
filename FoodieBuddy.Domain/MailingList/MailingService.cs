using System;
using FoodieBuddy.Domain.Models.MailingList;

namespace FoodieBuddy.Domain.MailingList
{
    public class MailingService: IMailingService
    {
        private IMailingRepository mailingRepository;

        public MailingService(IMailingRepository mailingRepository)
        {
            this.mailingRepository = mailingRepository;
        }

        public Mail Save(Guid id, Mail email)
        {
            if (String.IsNullOrEmpty(email.Email))
            {
                throw new EmailRequiredException("Email is required for mailing.");
            }

            Mail result = null;

            var foundExistingMail = mailingRepository.Retrieve(id);

            if (foundExistingMail == null)
            {
                result = mailingRepository.Create(email);
            }
            else
            {
                result = mailingRepository.Update(id, email);
            }

            return result;
        }
    }
}