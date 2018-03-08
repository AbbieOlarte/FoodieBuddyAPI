using FoodieBuddy.Domain.MailingList;
using FoodieBuddy.Domain.Models.MailingList;

namespace FoodieBuddy.Infrastructure.Persistence.Repositories
{
    public class MailingRepository : RepositoryBase<Mail>, IMailingRepository
    {
        public MailingRepository(IFoodieBuddyDbContext context) : base(context)
        {
        }
    }
}