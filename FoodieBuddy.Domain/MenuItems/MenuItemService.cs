using System;
using FoodieBuddy.Domain.Models.MenuItems;

namespace FoodieBuddy.Domain.MenuItems
{
    public class MenuItemService: IMenuItemService
    {
        private IMenuItemRepository menuItemRepository;

        public MenuItemService(IMenuItemRepository menuItemRepository)
        {
            this.menuItemRepository = menuItemRepository;
        }

        public MenuItem Save(Guid id, MenuItem foodItem)
        {
            if (String.IsNullOrEmpty(foodItem.FoodName))
            {
                throw new FoodNameRequiredException("Food name is required for Menu.");
            }
            if (String.IsNullOrEmpty(foodItem.Ingridients))
            {
                throw new IngredientsRequiredException("Ingredients are required for the Menu.");
            }
            if (foodItem.Price <= 0)
            {
                throw new PriceRequiredException("Price is required.");
            }

            MenuItem result = null;

            var foundMenuitem = menuItemRepository.Retrieve(id);

            if (foundMenuitem == null)
            {
                result = menuItemRepository.Create(foodItem);
            }
            else
            {
                result = menuItemRepository.Update(id, foodItem);
            }

            return result;
        }
    }
}