using AuthMaster.Dtos;
using AuthMaster.Model;
using AuthMaster.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;
using TestAuthJWT.Data;

namespace AuthMaster.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly CartServices _services;
        private readonly DataContext _dataContext;

        public CartController(CartServices services, DataContext dataContext)
        {
            _services = services;
            _dataContext = dataContext;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Add(ItemsDto dto)
        {
            var selectedProduct = await _dataContext.products.FindAsync(dto.ProductId);
            if (selectedProduct == null)
            {
                return BadRequest("Selected product does not exist.");
            }

            var userCart = await _services.GetUserCart();
            if (userCart == null)
            {
                userCart = new Cart
                {
                    userID = (Guid)_services.GetCurrentUserId(),
                    CartItems = new List<CartItems>(),
                };
                _dataContext.carts.Add(userCart);
                await _dataContext.SaveChangesAsync();
            }

            if (dto.Quantity == 0)           
                 return BadRequest ("Quantity must be greater than 0.");
            
            else if (dto.Quantity > selectedProduct.Quantity)           
                return BadRequest ($"Only {selectedProduct.Quantity} available.");
            

            var cartItem = new CartItems
            {
                ProductId = selectedProduct.ProductId,
                Quantity = dto.Quantity,
                price = selectedProduct.Price,
            };
             
            userCart.CartItems.Add(cartItem);

            var cart = new Cart
            {
                CartItems = userCart.CartItems,
                userID = (Guid)_services.GetCurrentUserId(),
            };

            selectedProduct.Quantity -= dto.Quantity;
            _dataContext.products.Update(selectedProduct);
            await _dataContext.SaveChangesAsync();

            return Ok(cartItem);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var userId = _services.GetCurrentUserId();
            var userCart = await _services.GetUserCart();

            if (userId.HasValue && userCart.userID == userId.Value)
            {
                await _services.DeleteItem(id);

                if (userCart.CartItems.Count == 0)
                {
                    _dataContext.carts.Remove(userCart);
                    await _dataContext.SaveChangesAsync();
                    return Ok("You don't have any items in your cart!");
                }                   
            } 
            else
            { 
                return Unauthorized();
            }
            return Ok(userCart);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetMyCart()
        {
            var userId = _services.GetCurrentUserId();
            var Usercart = await _services.GetUserCart();

            if (userId.HasValue)
            {
                if (Usercart is not null)
                {
                    return Ok(Usercart.CartItems);
                }
                else
                {
                    return Ok("You don't have a cart.");
                }
            }
            else
            {
                return Unauthorized("You don't have permission to access this user cart");
            }
        }


    }

}