using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using AvinaShop.Repository.IRepository;
using AvinaShop.Data;
using AvinaShop.Utility;

namespace AvinaShop.Services.OrderServices
{
    /// <summary>
    /// Service responsible for handling orders, including retrieving and updating order data.
    /// </summary>
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderService"/> class.
        /// </summary>
        /// <param name="orderRepository">The order repository instance for data access.</param>
        /// <param name="authenticationStateProvider">The authentication state provider for retrieving user information.</param>
        public OrderService(IOrderRepository orderRepository, AuthenticationStateProvider authenticationStateProvider)
        {
            _orderRepository = orderRepository;
            _authenticationStateProvider = authenticationStateProvider;
        }

        /// <summary>
        /// Retrieves a list of order headers based on the current user's role.
        /// Admin users receive all orders; regular users receive only their own orders.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation, containing a collection of order headers.</returns>
        public async Task<IEnumerable<OrderHeader>> GetOrderHeadersAsync()
        {
            var user = await GetCurrentUserAsync();

            // Admin users can view all orders, while regular users can only view their own orders
            if (IsAdmin(user))
            {
                return await GetAllOrdersAsync();
            }

            var userId = GetUserId(user);
            return await GetUserOrdersAsync(userId);
        }

        /// <summary>
        /// Retrieves the currently authenticated user.
        /// </summary>
        /// <returns>The currently authenticated user.</returns>
        private async Task<ClaimsPrincipal> GetCurrentUserAsync()
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            return authState.User;
        }

        /// <summary>
        /// Determines if the user is an administrator.
        /// </summary>
        /// <param name="user">The user to check.</param>
        /// <returns>True if the user is an admin; otherwise, false.</returns>
        private bool IsAdmin(ClaimsPrincipal user)
        {
            return user.IsInRole(AppConstants.Role_Admin);
        }

        /// <summary>
        /// Retrieves all order headers from the repository.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation, containing all order headers.</returns>
        private async Task<IEnumerable<OrderHeader>> GetAllOrdersAsync()
        {
            return await _orderRepository.GetAllAsync();
        }

        /// <summary>
        /// Retrieves order headers specific to a user.
        /// </summary>
        /// <param name="userId">The user ID to filter orders by.</param>
        /// <returns>A task that represents the asynchronous operation, containing the user's orders.</returns>
        private async Task<IEnumerable<OrderHeader>> GetUserOrdersAsync(string userId)
        {
            return await _orderRepository.GetAllAsync(userId);
        }

        /// <summary>
        /// Retrieves a specific order by its ID.
        /// </summary>
        /// <param name="id">The order ID.</param>
        /// <returns>A task that represents the asynchronous operation, containing the order details.</returns>
        public async Task<OrderHeader?> GetOrderByIdAsync(int id)
        {
            return await _orderRepository.GetAsync(id);
        }

        /// <summary>
        /// Updates the status of an order.
        /// </summary>
        /// <param name="id">The order ID to update.</param>
        /// <param name="status">The new status to set.</param>
        /// <param name="sessionId">The session ID for tracking the status update.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task UpdateOrderStatusAsync(int id, string status, string sessionId)
        {
            await _orderRepository.UpdateStatusAsync(id, status, sessionId);
        }

        /// <summary>
        /// Retrieves the user ID from the current authenticated user.
        /// </summary>
        /// <param name="user">The authenticated user.</param>
        /// <returns>The user ID.</returns>
        private string GetUserId(ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
        }
        public async Task CreateOrderAsync(OrderHeader orderHeader)
        {
            await _orderRepository.CreateAsync(orderHeader);
        }
    }
}

/*
 
 SOLID Principle                      | Implemented? | Notes
Single Responsibility Principle (SRP) | Yes          | The class manages only order-related logic (retrieving, updating orders). No other concerns are handled in this class.
Open/Closed Principle (OCP)           | Yes          | The service allows adding new methods without modifying existing ones. New features can be added by extending the service or repository.
Liskov Substitution Principle (LSP)   | Yes          | The class can be replaced by another class implementing the IOrderService interface without breaking functionality.
Interface Segregation Principle (ISP) | Yes          | The IOrderService interface seems focused on order management. However, as the project grows, it may need to be split into smaller interfaces.
Dependency Inversion Principle (DIP)  | Yes          | Dependencies like IOrderRepository and AuthenticationStateProvider are injected via constructor, ensuring low coupling and high flexibility.
 
 */