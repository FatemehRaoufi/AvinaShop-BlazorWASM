using AvinaShop.Data;
using AvinaShop.Repository;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AvinaShop.Tests.Repositories
{
    public class ShoppingCartRepositoryTests
    {
        private readonly ApplicationDbContext _context;
        private readonly ShoppingCartRepository _repository;

        public ShoppingCartRepositoryTests()
        {
            // Set up an in-memory database for isolated, repeatable tests
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid()) // Ensure unique DB per test class instance
                .Options;

            _context = new ApplicationDbContext(options);
            _repository = new ShoppingCartRepository(_context);
        }

        // Helper method (optional): returns all cart items for a user
        public async Task<IEnumerable<ShoppingCart>> GetAllAsync(string? userId)
        {
            if (string.IsNullOrEmpty(userId))
                return Enumerable.Empty<ShoppingCart>();

            return await _context.ShoppingCart
                .Where(cart => cart.UserId == userId)
                .ToListAsync();
        }

        [Fact]
        public async Task GetTotalCartCartCountAsync_Should_Return_Total_Count_For_User()
        {
            // Arrange: Add multiple items to the user's cart
            var userId = "user1";
            _context.ShoppingCart.AddRange(
                new ShoppingCart { UserId = userId, ProductId = 1, Count = 2 },
                new ShoppingCart { UserId = userId, ProductId = 2, Count = 3 }
            );
            await _context.SaveChangesAsync();

            // Act: Get total item count for user
            var result = await _repository.GetTotalCartCartCountAsync(userId);

            // Assert: Verify total count is correct
            Assert.Equal(5, result);
        }

        [Fact]
        public async Task UpdateCartAsync_Should_Add_New_Item_If_Not_Exists()
        {
            // Arrange: No item in cart yet
            var userId = "user1";
            var productId = 1;
            var updateBy = 2;

            // Act: Add new item
            var result = await _repository.UpdateCartAsync(userId, productId, updateBy);

            // Assert: Item should be added with correct count
            Assert.True(result);
            var cartItem = _context.ShoppingCart.FirstOrDefault(c => c.UserId == userId && c.ProductId == productId);
            Assert.NotNull(cartItem);
            Assert.Equal(updateBy, cartItem.Count);
        }

        [Fact]
        public async Task UpdateCartAsync_Should_Update_Existing_Item()
        {
            // Arrange: Item already exists with count = 2
            var userId = "user1";
            var productId = 1;
            _context.ShoppingCart.Add(new ShoppingCart { UserId = userId, ProductId = productId, Count = 2 });
            await _context.SaveChangesAsync();

            // Act: Update item count by +3
            var result = await _repository.UpdateCartAsync(userId, productId, 3);

            // Assert: Count should now be 5
            Assert.True(result);
            var cartItem = _context.ShoppingCart.FirstOrDefault(c => c.UserId == userId && c.ProductId == productId);
            Assert.NotNull(cartItem);
            Assert.Equal(5, cartItem.Count);
        }

        [Fact]
        public async Task UpdateCartAsync_Should_Remove_Item_If_Count_Becomes_Zero()
        {
            // Arrange: Item with count = 2 exists
            var userId = "user1";
            var productId = 1;
            _context.ShoppingCart.Add(new ShoppingCart { UserId = userId, ProductId = productId, Count = 2 });
            await _context.SaveChangesAsync();

            // Act: Reduce count by -2 → should result in removal
            var result = await _repository.UpdateCartAsync(userId, productId, -2);

            // Assert: Item should be removed
            Assert.True(result);
            var cartItem = _context.ShoppingCart.FirstOrDefault(c => c.UserId == userId && c.ProductId == productId);
            Assert.Null(cartItem);
        }

        [Fact]
        public async Task UpdateCartAsync_Should_Return_False_If_UserId_Is_Null()
        {
            // Act: Try updating cart without a user ID
            var result = await _repository.UpdateCartAsync(null, 1, 2);

            // Assert: Should return false due to invalid input
            Assert.False(result);
        }
    }
}
