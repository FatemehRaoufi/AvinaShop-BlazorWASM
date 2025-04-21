using AvinaShop.Data;
using AvinaShop.Repository;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AvinaShop.Tests.Repository
{
    public class OrderRepositoryTests
    {
        private readonly OrderRepository _orderRepository;
        private readonly ApplicationDbContext _dbContext;

        public OrderRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _dbContext = new ApplicationDbContext(options);
            _orderRepository = new OrderRepository(_dbContext);
        }

        [Fact]
        public async Task CreateAsync_ShouldAddOrder()
        {
            // Arrange
            var order = new OrderHeader
            {
                Id = 1,
                UserId = "user1",
                OrderTotal = 100.0,
                Status = "Pending",
                Name = "John Doe",
                PhoneNumber = "1234567890",
                Email = "john.doe@example.com"
            };

            // Act
            var result = await _orderRepository.CreateAsync(order);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(order.Id, result.Id);
            Assert.Equal(order.UserId, result.UserId);
            Assert.Equal(order.Status, result.Status);
        }

        [Fact]
        public async Task GetAsync_ShouldReturnOrderById()
        {
            // Arrange
            var order = new OrderHeader
            {
                Id = 2,
                UserId = "user2",
                OrderTotal = 200.0,
                Status = "Pending",
                Name = "Jane Doe",
                PhoneNumber = "9876543210",
                Email = "jane.doe@example.com"
            };
            _dbContext.OrderHeader.Add(order);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _orderRepository.GetAsync(order.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(order.Id, result.Id);
            Assert.Equal(order.UserId, result.UserId);
        }

        [Fact]
        public async Task UpdateStatusAsync_ShouldUpdateOrderStatus()
        {
            // Arrange
            var order = new OrderHeader
            {
                Id = 3,
                UserId = "user3",
                OrderTotal = 300.0,
                Status = "Pending",
                Name = "Alice",
                PhoneNumber = "5555555555",
                Email = "alice@example.com"
            };
            _dbContext.OrderHeader.Add(order);
            await _dbContext.SaveChangesAsync();

            // Act
            var updatedOrder = await _orderRepository.UpdateStatusAsync(order.Id, "Completed", "payment123");

            // Assert
            Assert.NotNull(updatedOrder);
            Assert.Equal("Completed", updatedOrder.Status);
            Assert.Equal("payment123", updatedOrder.PaymentIntentId);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllOrders()
        {
            // Arrange
            _dbContext.OrderHeader.RemoveRange(_dbContext.OrderHeader); // Clear existing data
            await _dbContext.SaveChangesAsync();

            var orders = new List<OrderHeader>
            {
                new OrderHeader { Id = 4, UserId = "user4", OrderTotal = 400.0, Status = "Pending", Name = "Bob", PhoneNumber = "1111111111", Email = "bob@example.com" },
                new OrderHeader { Id = 5, UserId = "user5", OrderTotal = 500.0, Status = "Pending", Name = "Charlie", PhoneNumber = "2222222222", Email = "charlie@example.com" }
            };
            _dbContext.OrderHeader.AddRange(orders);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _orderRepository.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

    }
}
