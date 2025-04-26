using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using AvinaShop.Repository.IRepository;
using AvinaShop.Utility;
using AvinaShop.Data;


namespace AvinaShop.Services.OrderServices
{
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
        // Retrieve the current user's authentication state
        var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;

        // If the user is an admin, return all orders
        if (user.IsInRole(AppConstants.Role_Admin))
        {
            return await _orderRepository.GetAllAsync();
        }

        // Otherwise, return orders specific to the user
        var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return await _orderRepository.GetAllAsync(userId);
    }
}

}
/*

SOLID Principle | Code Status | Explanation
   S - Single Responsibility | Compliant | OrderService only handles fetching Orders, with no other responsibilities.
   O - Open/Closed Principle | Compliant | If needed, you can change the way orders are fetched without modifying existing code.
   L - Liskov Substitution | Compliant | Since an Interface (IOrderService) is used, any other class that follows the contract can replace it.
   I - Interface Segregation | Compliant | The Interface defines only the method you need (fetching orders), not any unnecessary ones.
   D - Dependency Inversion | Compliant | The code depends on the Interface (IOrderService), not directly on a specific class (like OrderRepository). 
 
 */