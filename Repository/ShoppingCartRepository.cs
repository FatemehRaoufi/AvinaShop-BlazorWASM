using AvinaShop.Data;
using AvinaShop.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace AvinaShop.Repository
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly ApplicationDbContext _db;

        public ShoppingCartRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Retrieves all shopping cart items for a specific user, including product details.
        /// </summary>
        public async Task<IEnumerable<ShoppingCart>> GetItemsAsync(string userId)
        {
            return await _db.ShoppingCart
                .Where(c => c.UserId == userId)
                .Include(c => c.Product)
                .ToListAsync();
        }
        public async Task<IEnumerable<ShoppingCart>> GetAllAsync(string userId)
        {
           
            return await _db.ShoppingCart.Where(sc => sc.UserId == userId).ToListAsync();

            
        }

        /// <summary>
        /// Calculates the total quantity of items in the user's shopping cart.
        /// </summary>
        public async Task<int> GetTotalCartCountAsync(string userId)
        {
            return await _db.ShoppingCart
                .Where(c => c.UserId == userId)
                .SumAsync(c => c.Count);
        }

        /// <summary>
        /// Adds a new product to the user's cart if it doesn't already exist.
        /// </summary>
        /// <returns>True if added successfully; false if the item exists or count is invalid.</returns>
        public async Task<bool> AddItemToCartAsync(string userId, int productId, int count)
        {
            if (count <= 0) return false;

            var existing = await _db.ShoppingCart
                .FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == productId);

            if (existing != null) return false; // Prevent duplicate items

            var cart = new ShoppingCart
            {
                UserId = userId,
                ProductId = productId,
                Count = count
            };

            await _db.ShoppingCart.AddAsync(cart);
            return await _db.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// Increases the quantity of a specific product in the user's cart.
        /// </summary>
        /// <returns>True if updated successfully; false if the item doesn't exist or input is invalid.</returns>
        public async Task<bool> IncreaseItemQuantityAsync(string userId, int productId, int amount)
        {
            if (amount <= 0) return false;

            var cart = await _db.ShoppingCart
                .FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == productId);

            if (cart == null) return false;

            cart.Count += amount;
            return await _db.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// Decreases the quantity of a product in the user's cart.
        /// Removes the item if quantity drops to zero or below.
        /// </summary>
        public async Task<bool> DecreaseItemQuantityAsync(string userId, int productId, int amount)
        {
            if (amount <= 0) return false;

            var cart = await _db.ShoppingCart
                .FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == productId);

            if (cart == null) return false;

            cart.Count -= amount;

            if (cart.Count <= 0)
            {
                _db.ShoppingCart.Remove(cart); // Auto-remove if count becomes non-positive
            }

            return await _db.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// Removes a specific product from the user's cart.
        /// </summary>
        public async Task<bool> RemoveItemFromCartAsync(string userId, int productId)
        {
            var cart = await _db.ShoppingCart
                .FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == productId);

            if (cart == null) return false;

            _db.ShoppingCart.Remove(cart);
            return await _db.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// Clears all items from the user's shopping cart.
        /// </summary>
        public async Task<bool> ClearCartAsync(string userId)
        {
            var items = await _db.ShoppingCart
                .Where(c => c.UserId == userId)
                .ToListAsync();

            _db.ShoppingCart.RemoveRange(items);
            return await _db.SaveChangesAsync() > 0;
        }
    }
}
