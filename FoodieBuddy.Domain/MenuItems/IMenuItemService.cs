using FoodieBuddy.Domain.Models.MenuItems;
using System;

namespace FoodieBuddy.Domain.MenuItems
{
    public interface IMenuItemService
    {
        MenuItem Save(Guid id, MenuItem foodItem);
    }
}