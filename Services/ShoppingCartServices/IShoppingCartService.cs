using System.Collections.Generic;
using System.Threading.Tasks;
using AvinaShop.Data;

namespace AvinaShop.Services.ShoppingCartServices
{
    public interface IShoppingCartService
    {


        /// <summary>
        /// Adds an item to the user's cart if it doesn't already exist.
        /// If the item already exists in the cart, no action is taken.
        /// </summary>
        /// <param name="userId">The ID of the user whose cart is being updated.</param>
        /// <param name="productId">The ID of the product to be added to the cart.</param>
        /// <param name="quantity">The quantity of the product to be added.</param>
        /// <returns>True if the item was successfully added, false otherwise (e.g., if the item already exists or the quantity is invalid).</returns>
        Task<bool> AddItemToCartAsync(string userId, int productId, int quantity);

        /// <summary>
        /// Updates the quantity of an item in the user's cart.
        /// This method allows for increasing or decreasing the quantity of an item in the cart.
        /// If the item does not exist in the cart, no action will be taken.
        /// </summary>
        /// <param name="userId">The ID of the user whose cart is being updated.</param>
        /// <param name="productId">The ID of the product whose quantity will be updated.</param>
        /// <param name="quantityChange">The change in quantity, which can be positive (increase) or negative (decrease).</param>
        /// <returns>True if the quantity was successfully updated, false if the item does not exist or the quantity change is invalid.</returns>
        Task<bool> UpdateItemQuantityAsync(string userId, int productId, int quantityChange);

        /// <summary>
        /// Retrieves all items in the user's shopping cart.
        /// This method will return the complete list of items currently in the user's cart.
        /// </summary>
        /// <param name="userId">The ID of the user whose cart items will be fetched.</param>
        /// <returns>A list of shopping cart items for the specified user.</returns>
        Task<IEnumerable<ShoppingCart>> GetUserCartAsync(string userId);

        /// <summary>
        /// Gets the total count of items in the user's cart.
        /// This method returns the total number of items (across all products) currently in the user's cart.
        /// </summary>
        /// <param name="userId">The ID of the user whose cart item count will be fetched.</param>
        /// <returns>The total count of items in the user's cart.</returns>
        Task<int> GetCartItemCountAsync(string userId);

        /// <summary>
        /// Clears all items from the user's shopping cart.
        /// This method will remove every item currently in the user's cart, effectively clearing it.
        /// </summary>
        /// <param name="userId">The ID of the user whose cart will be cleared.</param>
        /// <returns>True if the cart was successfully cleared, false if there was an error.</returns>
        Task<bool> ClearCartAsync(string userId);
        Task<bool> RemoveItemFromCartAsync(string userId, int productId);

    }
}
