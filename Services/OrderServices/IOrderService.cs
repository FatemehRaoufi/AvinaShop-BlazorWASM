
using AvinaShop.Data;

namespace AvinaShop.Services.OrderServices
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderHeader>> GetOrderHeadersAsync();

    }
}

