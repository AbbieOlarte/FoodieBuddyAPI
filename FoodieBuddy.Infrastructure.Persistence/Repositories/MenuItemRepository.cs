using FoodieBuddy.Domain.MenuItems;
using FoodieBuddy.Domain.Models.MenuItems;

namespace FoodieBuddy.Infrastructure.Persistence.Repositories
{
    public class MenuItemRepository : RepositoryBase<MenuItem>, IMenuItemRepository
    {
        public MenuItemRepository(IFoodieBuddyDbContext context) : base(context)
        {
        }
    }
}