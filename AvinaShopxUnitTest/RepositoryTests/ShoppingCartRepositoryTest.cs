using AvinaShop.Repository;
using AvinaShop.Data;
using Moq;
using Xunit;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AvinaShop.Tests.Repository
{
    public class ShoppingCartRepositoryTests
    {
        private readonly Mock<DbSet<ShoppingCart>> _mockShoppingCartDbSet;
        private readonly Mock<ApplicationDbContext> _mockDbContext;
        private readonly ShoppingCartRepository _shoppingCartRepository;

        public ShoppingCartRepositoryTests()
        {
            // Mock DbSet for ShoppingCart
            _mockShoppingCartDbSet = new Mock<DbSet<ShoppingCart>>();

            // Mock the ApplicationDbContext
            _mockDbContext = new Mock<ApplicationDbContext>();
            _mockDbContext.Setup(m => m.ShoppingCart).Returns(_mockShoppingCartDbSet.Object);

            // Initialize the repository with the mock DbContext
            _shoppingCartRepository = new ShoppingCartRepository(_mockDbContext.Object);
        }

        [Fact]
        public async Task GetItemsAsync_ReturnsShoppingCartItems_WhenUserIdIsValid()
        {
            // Arrange
            var userId = "user123";
            var shoppingCarts = new List<ShoppingCart>
            {
                new ShoppingCart { UserId = userId, ProductId = 1, Count = 2 },
                new ShoppingCart { UserId = userId, ProductId = 2, Count = 3 }
            }.AsQueryable();

            var mockDbSet = shoppingCarts.BuildMockDbSet();
            _mockDbContext.Setup(x => x.ShoppingCart.Where(It.IsAny<System.Linq.Expressions.Expression<System.Func<ShoppingCart, bool>>>())).Returns(mockDbSet.Object);

            // Act
            var result = await _shoppingCartRepository.GetItemsAsync(userId);

            // Assert
            Assert.Equal(2, result.Count()); // Check that two items are returned
            Assert.Equal(userId, result.First().UserId); // Check that the user ID matches
        }

        [Fact]
        public async Task AddItemToCartAsync_ReturnsFalse_WhenItemExists()
        {
            // Arrange
            var userId = "user123";
            var productId = 1;
            var count = 5;

            var shoppingCart = new ShoppingCart { UserId = userId, ProductId = productId, Count = count };

            var shoppingCarts = new List<ShoppingCart> { shoppingCart }.AsQueryable();
            var mockDbSet = shoppingCarts.BuildMockDbSet();
            _mockDbContext.Setup(m => m.ShoppingCart).Returns(mockDbSet.Object);

            // Act
            var result = await _shoppingCartRepository.AddItemToCartAsync(userId, productId, count);

            // Assert
            Assert.False(result); // It should return false because the item already exists
        }

        [Fact]
        public async Task AddItemToCartAsync_ReturnsTrue_WhenItemIsNew()
        {
            // Arrange
            var userId = "user123";
            var productId = 1;
            var count = 5;

            var shoppingCarts = new List<ShoppingCart>().AsQueryable();
            var mockDbSet = shoppingCarts.BuildMockDbSet();
            _mockDbContext.Setup(m => m.ShoppingCart).Returns(mockDbSet.Object);

            // Act
            var result = await _shoppingCartRepository.AddItemToCartAsync(userId, productId, count);

            // Assert
            Assert.True(result); // It should return true because the item is added
        }

        [Fact]
        public async Task IncreaseItemQuantityAsync_ReturnsTrue_WhenItemIsUpdated()
        {
            // Arrange
            var userId = "user123";
            var productId = 1;
            var increaseAmount = 2;

            var shoppingCart = new ShoppingCart { UserId = userId, ProductId = productId, Count = 5 };
            var shoppingCarts = new List<ShoppingCart> { shoppingCart }.AsQueryable();

            var mockDbSet = shoppingCarts.BuildMockDbSet();
            _mockDbContext.Setup(m => m.ShoppingCart).Returns(mockDbSet.Object);

            // Act
            var result = await _shoppingCartRepository.IncreaseItemQuantityAsync(userId, productId, increaseAmount);

            // Assert
            Assert.True(result); // It should return true as the quantity is increased
            Assert.Equal(7, shoppingCart.Count); // Check if the quantity is updated correctly
        }

        [Fact]
        public async Task DecreaseItemQuantityAsync_ReturnsFalse_WhenItemDoesNotExist()
        {
            // Arrange
            var userId = "user123";
            var productId = 1;
            var decreaseAmount = 2;

            var shoppingCarts = new List<ShoppingCart>().AsQueryable();
            var mockDbSet = shoppingCarts.BuildMockDbSet();
            _mockDbContext.Setup(m => m.ShoppingCart).Returns(mockDbSet.Object);

            // Act
            var result = await _shoppingCartRepository.DecreaseItemQuantityAsync(userId, productId, decreaseAmount);

            // Assert
            Assert.False(result); // Should return false as the item doesn't exist
        }

        [Fact]
        public async Task RemoveItemFromCartAsync_ReturnsTrue_WhenItemIsRemoved()
        {
            // Arrange
            var userId = "user123";
            var productId = 1;

            var shoppingCart = new ShoppingCart { UserId = userId, ProductId = productId, Count = 5 };
            var shoppingCarts = new List<ShoppingCart> { shoppingCart }.AsQueryable();

            var mockDbSet = shoppingCarts.BuildMockDbSet();
            _mockDbContext.Setup(m => m.ShoppingCart).Returns(mockDbSet.Object);

            // Act
            var result = await _shoppingCartRepository.RemoveItemFromCartAsync(userId, productId);

            // Assert
            Assert.True(result); // Should return true because the item is removed
        }

        [Fact]
        public async Task ClearCartAsync_ReturnsTrue_WhenCartIsCleared()
        {
            // Arrange
            var userId = "user123";
            var shoppingCarts = new List<ShoppingCart>
            {
                new ShoppingCart { UserId = userId, ProductId = 1, Count = 2 },
                new ShoppingCart { UserId = userId, ProductId = 2, Count = 3 }
            }.AsQueryable();

            var mockDbSet = shoppingCarts.BuildMockDbSet();
            _mockDbContext.Setup(m => m.ShoppingCart).Returns(mockDbSet.Object);

            // Act
            var result = await _shoppingCartRepository.ClearCartAsync(userId);

            // Assert
            Assert.True(result); // It should return true because the cart is cleared
        }

        // Helper method to mock DbSet
        public static class MockDbSetExtension
        {
            public static Mock<DbSet<T>> BuildMockDbSet<T>(this IQueryable<T> source) where T : class
            {
                var mockDbSet = new Mock<DbSet<T>>();
                mockDbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(source.Provider);
                mockDbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(source.Expression);
                mockDbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(source.ElementType);
                mockDbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(source.GetEnumerator());
                return mockDbSet;
            }
        }
    }
}
