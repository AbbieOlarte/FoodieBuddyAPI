using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodieBuddy.API.Utils;
using FoodieBuddy.Domain.MenuItems;
using FoodieBuddy.Domain.Models.MenuItems;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace FoodieBuddy.API.Controllers
{
    [Produces("application/json")]
    [Route("api/MenuItems")]
    public class MenuItemsController : Controller
    {
        private IMenuItemRepository menuItemRepository;
        private IMenuItemService menuItemService;

        public MenuItemsController(IMenuItemRepository menuItemRepository, IMenuItemService menuItemService)
        {
            this.menuItemRepository = menuItemRepository;
            this.menuItemService = menuItemService;
        }

        [HttpGet, ActionName("GetMenuItems")]
        public IActionResult GetMenuItem(Guid? id)
        {
            var result = new List<MenuItem>();
            if (id == null)
            {
                result.AddRange(this.menuItemRepository.Retrieve());
            }
            else
            {
                var foodItem = menuItemRepository.Retrieve(id.Value);
                result.Add(foodItem);
            }
            return Ok(result);
        }

        [HttpPost]
        public IActionResult CreateMenuItem([FromBody]MenuItem foodItem)
        {
            try
            {
                if (foodItem == null)
                {
                    return BadRequest();
                }
                var result = this.menuItemService.Save(Guid.Empty, foodItem);
                return CreatedAtAction("GetMenuItems", new { id = foodItem.FoodId }, foodItem);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpDelete]
        public IActionResult DeleteMenuItem(Guid id)
        {
            var foodItemToDelete = this.menuItemRepository.Retrieve(id);
            if (foodItemToDelete == null)
            {
                return NotFound();
            }
            this.menuItemRepository.Delete(id);
            return NoContent();
        }

        [HttpPut]
        public IActionResult UpdateMenuItem(Guid id, [FromBody] MenuItem foodItem)
        {
            try
            {
                if (foodItem == null)
                {
                    return BadRequest();
                }
                var foodItemToUpdate = this.menuItemRepository.Retrieve(id);
                if (foodItemToUpdate == null)
                {
                    return NotFound();
                }
                foodItemToUpdate.ApplyChanges(foodItem);
                var result = this.menuItemService.Save(id, foodItemToUpdate);
                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPatch]
        public IActionResult PatchMenuItem(Guid id, JsonPatchDocument patchedFoodItem)
        {
            if (patchedFoodItem == null)
            {
                return BadRequest();
            }
            var foodItem = this.menuItemRepository.Retrieve(id);
            if (foodItem == null)
            {
                return NotFound();
            }
            patchedFoodItem.ApplyTo(foodItem);
            var result = this.menuItemService.Save(id, foodItem);
            return Ok(result);
        }
    }
}