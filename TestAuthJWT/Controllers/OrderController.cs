using AuthMaster.Model;
using AuthMaster.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestAuthJWT.Data;

namespace AuthMaster.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly CartServices _cartServices;
        private readonly OrderService _orderService;  
        private readonly DataContext _dataContext;

        public OrderController(CartServices cartServices, OrderService orderService, DataContext dataContext)
        {
            _cartServices = cartServices;
            _orderService = orderService;
            _dataContext = dataContext;
        }

        [HttpGet]
        [Authorize]       
        public async Task<IActionResult> PlaceOrder(int cartId, string address)
        {
            var userId = _cartServices.GetCurrentUserId();
            var userCart = await _orderService.GetUserCartForOrder(cartId);

            if (userCart == null || userCart.userID != userId)
            {
                return Unauthorized("Unable to place This order..");
            }

            var order = await _orderService.PlaceOrder(cartId, address);

            if (order == null)
            {
                return BadRequest("Unable to place This order.");
            } 
            
            _dataContext.carts.Remove(userCart);
            await _dataContext.SaveChangesAsync();
            return Ok($"Order Placed Success And the Total Price is {order.TotalPrice}");
        }
    }
}
