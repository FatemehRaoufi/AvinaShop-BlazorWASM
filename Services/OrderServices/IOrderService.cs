using AvinaShop.Data;

namespace AvinaShop.Services.OrderServices
{
    public interface IOrderService
    {
        /// <summary>
        /// Retrieves a list of order headers based on the current user's role.
        /// Admin users receive all orders; regular users receive only their own orders.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation, containing a collection of order headers.</returns>
        Task<IEnumerable<OrderHeader>> GetOrderHeadersAsync();

        /// <summary>
        /// Retrieves an order header by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the order.</param>
        /// <returns>A task that represents the asynchronous operation, containing the order header.</returns>
        Task<OrderHeader?> GetOrderByIdAsync(int id);

        /// <summary>
        /// Updates the status of an order based on the order ID and the new status.
        /// </summary>
        /// <param name="id">The unique identifier of the order to update.</param>
        /// <param name="status">The new status to assign to the order.</param>
        /// <param name="sessionId">The session ID related to the status change.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task UpdateOrderStatusAsync(int id, string status, string sessionId);

        /// <summary>
        /// Creates a new order based on the provided order header and associated details.
        /// </summary>
        /// <param name="orderHeader">The order header containing user information, items, and total amount.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task CreateOrderAsync(OrderHeader orderHeader);

        
    }
}
