using AvinaShop.Data;
using AvinaShop.Repository;
using AvinaShop.Repository.IRepository;
using AvinaShop.Services.ShoppingCartServices;

public class ShoppingCartService : IShoppingCartService
{
    private readonly IShoppingCartRepository _cartRepo;

    public ShoppingCartService(IShoppingCartRepository cartRepo)
    {
        _cartRepo = cartRepo;
    }

    // Retrieves all items in the user's shopping cart.
    public async Task<IEnumerable<ShoppingCart>> GetUserCartAsync(string userId)
    {
       
        return await _cartRepo.GetAllAsync(userId);
    }

    // Adds a product to the user's cart if it's not already there.
    public async Task<bool> AddItemToCartAsync(string userId, int productId, int quantity)
    {
        if (string.IsNullOrEmpty(userId)) return false;
        if (quantity <= 0) return false;

        var cartItems = await _cartRepo.GetItemsAsync(userId);
        var existingItem = cartItems.FirstOrDefault(c => c.ProductId == productId);

        if (existingItem == null)
        {
            return await _cartRepo.AddItemToCartAsync(userId, productId, quantity);
        }

        return false;
    }

    // Updates the quantity of a specific item in the user's cart.
    public async Task<bool> UpdateItemQuantityAsync(string userId, int productId, int quantityChange)
    {
        if (string.IsNullOrEmpty(userId)) return false;
        if (quantityChange == 0) return true;

        var cartItems = await _cartRepo.GetItemsAsync(userId);
        var item = cartItems.FirstOrDefault(c => c.ProductId == productId);

        if (item == null) return false;

        if (quantityChange > 0)
        {
            return await _cartRepo.IncreaseItemQuantityAsync(userId, productId, quantityChange);
        }
        else
        {
            return await _cartRepo.DecreaseItemQuantityAsync(userId, productId, -quantityChange);
        }
    }

    // Gets the total count of items in the user's cart.
    public Task<int> GetCartItemCountAsync(string userId)
    {
        return _cartRepo.GetTotalCartCountAsync(userId);
    }

    // Clears all items from the user's cart.
    public Task<bool> ClearCartAsync(string userId)
    {
        return _cartRepo.ClearCartAsync(userId);
    }

    // Method to remove an item from the cart
    public async Task<bool> RemoveItemFromCartAsync(string userId, int productId)
    {
        if (string.IsNullOrEmpty(userId)) return false;

        var cartItems = await _cartRepo.GetItemsAsync(userId);
        var itemToRemove = cartItems.FirstOrDefault(c => c.ProductId == productId);

        if (itemToRemove != null)
        {
            // Call the repository to remove the item
            return await _cartRepo.RemoveItemFromCartAsync(userId, productId);
        }

        return false;
    }
}
