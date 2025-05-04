using Moq;
using Xunit;
using System.Security.Claims;
using System.Collections.Generic;
using System.Threading.Tasks;
using AvinaShop.Services.OrderServices;
using AvinaShop.Repository.IRepository;
using AvinaShop.Data;
using Microsoft.AspNetCore.Components.Authorization;

namespace AvinaShop.Tests
{
    public class OrderServiceTests
    {
        private readonly Mock<IOrderRepository> _mockOrderRepository;
        private readonly Mock<AuthenticationStateProvider> _mockAuthenticationStateProvider;
        private readonly OrderService _orderService;

        public OrderServiceTests()
        {
            _mockOrderRepository = new Mock<IOrderRepository>();
            _mockAuthenticationStateProvider = new Mock<AuthenticationStateProvider>();
            _orderService = new OrderService(_mockOrderRepository.Object, _mockAuthenticationStateProvider.Object);
        }

        [Fact]
        public async Task GetOrderHeadersAsync_AdminUser_ReturnsAllOrders()
        {
            // Arrange
            var mockClaimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Role, "Admin"),
            }));

            var authState = new AuthenticationState(mockClaimsPrincipal);
            _mockAuthenticationStateProvider
                .Setup(x => x.GetAuthenticationStateAsync())
                .Returns(Task.FromResult(authState));

            var orderHeaders = new List<OrderHeader>
            {
                new OrderHeader { Id = 1, UserId = "user1" },
                new OrderHeader { Id = 2, UserId = "user2" }
            };

            _mockOrderRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(orderHeaders);

            // Act
            var result = await _orderService.GetOrderHeadersAsync();

            // Assert
            Assert.Equal(2, result.Count());
            _mockOrderRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetOrderHeadersAsync_RegularUser_ReturnsUserOrders()
        {
            // Arrange
            var mockClaimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "user1"),
            }));

            var authState = new AuthenticationState(mockClaimsPrincipal);
            _mockAuthenticationStateProvider
                .Setup(x => x.GetAuthenticationStateAsync())
                .Returns(Task.FromResult(authState));

            var userOrderHeaders = new List<OrderHeader>
            {
                new OrderHeader { Id = 1, UserId = "user1" }
            };

            _mockOrderRepository.Setup(repo => repo.GetAllAsync("user1")).ReturnsAsync(userOrderHeaders);

            // Act
            var result = await _orderService.GetOrderHeadersAsync();

            // Assert
            Assert.Single(result);
            _mockOrderRepository.Verify(repo => repo.GetAllAsync("user1"), Times.Once);
        }
    }
}
