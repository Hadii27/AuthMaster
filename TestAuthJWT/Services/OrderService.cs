using AuthMaster.Model;
using Microsoft.EntityFrameworkCore;
using TestAuthJWT.Data;

namespace AuthMaster.Services
{
    public class OrderService
    {
        private readonly DataContext _dataContext;
        private readonly CartServices _cartServices;
        private readonly ProductService _productService;
        public OrderService(DataContext dataContext, CartServices cartServices, ProductService productService)
        {
            _dataContext = dataContext;
            _cartServices = cartServices;
            _productService = productService;
        }
        public async Task<Cart> GetUserCartForOrder(int cartId)
        {
            Guid? userId = _cartServices.GetCurrentUserId();

            if (!userId.HasValue)
            {
                return null;
            }

            var userCart = await _dataContext.carts
                                .Include(x => x.CartItems)
                                .SingleOrDefaultAsync(x => x.Id == cartId && x.userID == userId.Value);
            return userCart;
        }


        public async Task<Order> PlaceOrder(int cartId, string address)
        {
            decimal TotalOrderPrice = 0;
            decimal SubPrice = 0;

            var userCart = await GetUserCartForOrder(cartId);
            if (userCart == null)
            {
                return null;
            }
          
            var order = new Order
            {
                UserId = (Guid)_cartServices.GetCurrentUserId(),
                DateTime = DateTime.UtcNow,
                TotalPrice = TotalOrderPrice,
                 address = address,
                orderItems = new List<OrderItems>() 
            };

            foreach (var cartItem in userCart.CartItems)
            {
                var selectedProduct = await _productService.GetById(cartItem.ProductId);
                if (selectedProduct != null)
                {
                    SubPrice += (decimal)(selectedProduct.Price * cartItem.Quantity);

                    var orderItem = new OrderItems
                    {
                        productId = selectedProduct.ProductId,
                        PriceAtOrderTime = SubPrice,
                        Quantity = cartItem.Quantity,
                        OrderId = order.Id
                    };
                    TotalOrderPrice += SubPrice;
                    order.orderItems.Add(orderItem);
                }
            }

            _dataContext.orders.Add(order);
            order.TotalPrice = TotalOrderPrice;
            await _dataContext.SaveChangesAsync();
            return order;
        }
    }
}