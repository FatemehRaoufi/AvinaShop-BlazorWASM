using AvinaShop.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AvinaShop.Repository.IRepository
{
    public interface IShoppingCartRepository
    {

        /// <summary>
        /// Retrieves the shopping cart items for a specific user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user whose shopping cart is to be retrieved.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains a collection of <see cref="ShoppingCart"/> objects associated with the specified user.</returns>
        Task<IEnumerable<ShoppingCart>> GetAllAsync(string userId);


        /// <summary>
        /// Retrieves all shopping cart items for a specific user, including product details.
        /// </summary>
        /// <param name="userId">The ID of the user whose cart items are being retrieved.</param>
        /// <returns>A list of shopping cart items for the specified user.</returns>
        Task<IEnumerable<ShoppingCart>> GetItemsAsync(string userId);

        /// <summary>
        /// Calculates the total quantity of items in the user's shopping cart.
        /// </summary>
        /// <param name="userId">The ID of the user whose cart item count is to be calculated.</param>
        /// <returns>The total quantity of items in the user's cart.</returns>
        Task<int> GetTotalCartCountAsync(string userId);

        /// <summary>
        /// Adds a new product to the user's cart if it doesn't already exist.
        /// </summary>
        /// <param name="userId">The ID of the user adding the item to the cart.</param>
        /// <param name="productId">The ID of the product being added to the cart.</param>
        /// <param name="count">The quantity of the product being added to the cart.</param>
        /// <returns>True if the item was added successfully; false if the item already exists or the quantity is invalid.</returns>
        Task<bool> AddItemToCartAsync(string userId, int productId, int count);

        /// <summary>
        /// Increases the quantity of a specific product in the user's cart.
        /// </summary>
        /// <param name="userId">The ID of the user whose cart item quantity is being updated.</param>
        /// <param name="productId">The ID of the product whose quantity is being increased.</param>
        /// <param name="amount">The amount by which the quantity should be increased.</param>
        /// <returns>True if the quantity was updated successfully; false if the item doesn't exist or the amount is invalid.</returns>
        Task<bool> IncreaseItemQuantityAsync(string userId, int productId, int amount);

        /// <summary>
        /// Decreases the quantity of a product in the user's cart.
        /// Removes the item from the cart if the quantity drops to zero or below.
        /// </summary>
        /// <param name="userId">The ID of the user whose cart item quantity is being updated.</param>
        /// <param name="productId">The ID of the product whose quantity is being decreased.</param>
        /// <param name="amount">The amount by which the quantity should be decreased.</param>
        /// <returns>True if the quantity was updated successfully; false if the item doesn't exist or the amount is invalid.</returns>
        Task<bool> DecreaseItemQuantityAsync(string userId, int productId, int amount);

        /// <summary>
        /// Removes a specific product from the user's cart.
        /// </summary>
        /// <param name="userId">The ID of the user removing the item from their cart.</param>
        /// <param name="productId">The ID of the product being removed from the cart.</param>
        /// <returns>True if the item was removed successfully; false if the item doesn't exist in the cart.</returns>
        Task<bool> RemoveItemFromCartAsync(string userId, int productId);
       
        /// <summary>
        /// Clears all items from the user's shopping cart.
        /// </summary>
        /// <param name="userId">The ID of the user whose cart is being cleared.</param>
        /// <returns>True if the cart was cleared successfully; false if there was an issue clearing the cart.</returns>
        Task<bool> ClearCartAsync(string userId);

       
    }
}