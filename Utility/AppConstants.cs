
using AvinaShop.Data;

namespace AvinaShop.Utility
{
    public static class AppConstants
    {
        public static string Role_Admin = "Admin"; // Admin role identifier
        public static string Role_Customer = "Customer"; // Customer role identifier

        public static string StatusPending = "Pending";
        public static string StatusApproved = "Approved";
        public static string StatusReadyForPickUp = "ReadyForPickUp";
        public static string StatusCompleted = "Completed";
        public static string StatusCancelled = "Cancelled";

        public static List<OrderDetail> ConvertShoppingCartListToOrderDetail(List<ShoppingCart> shoppingCarts)
        {
            List<OrderDetail> orderDetails = new List<OrderDetail>();

            foreach (var cart in shoppingCarts)
            {
                OrderDetail orderDetail = new OrderDetail
                {
                    ProductId = cart.ProductId,
                    Count = cart.Count,
                    Price = Convert.ToDouble(cart.Product.Price),
                    ProductName = cart.Product.Name
                };
                orderDetails.Add(orderDetail);
            }

            return orderDetails;
        }

    }
}
