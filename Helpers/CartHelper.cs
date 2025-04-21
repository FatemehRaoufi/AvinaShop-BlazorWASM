using AvinaShop.Data;
using AvinaShop.Repository.IRepository;
using System.Collections.Generic;
using System.Linq;

namespace AvinaShop.Helpers
{
    public static class CartHelper
    {
        // Helper method to calculate the total amount for the cart
        public static double CalculateTotalAmount(IEnumerable<ShoppingCart> shoppingCarts)
        {
            double totalAmount = 0;
            foreach (var cart in shoppingCarts)
            {
                totalAmount += (Convert.ToDouble(cart.Product.Price) * cart.Count);
            }
            return Math.Round(totalAmount, 2); // Round total to 2 decimal places
        }

        // Helper method to calculate the total item count in the cart
        public static int CalculateTotalItems(IEnumerable<ShoppingCart> shoppingCarts)
        {
            return shoppingCarts.Sum(cart => cart.Count);
        }

        // Helper method to load cart items for a user
        public static async Task<IEnumerable<ShoppingCart>> LoadCartAsync(IShoppingCartRepository cartRepository, string userId)
        {
            return await cartRepository.GetAllAsync(userId);
        }

        // Helper method to update cart items count
        public static async Task UpdateCartItemAsync(IShoppingCartRepository cartRepository, string userId, int productId, int updateBy)
        {
            await cartRepository.UpdateCartAsync(userId, productId, updateBy);
        }
    }
}