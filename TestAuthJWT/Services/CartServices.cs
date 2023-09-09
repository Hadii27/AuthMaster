using AuthMaster.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using TestAuthJWT.Data;

namespace AuthMaster.Services
{
    public class CartServices
    {
        private readonly DataContext _dataContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartServices(DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _dataContext = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public Guid? GetCurrentUserId()
        {
            var userIdClaim = _httpContextAccessor.HttpContext.User.FindFirstValue("UserId");
            var UserId = Guid.TryParse(userIdClaim, out Guid userId);           
            return userId;
        }

        public async Task<Cart> GetUserCart()
        {
            Guid? userId = GetCurrentUserId();

            if (userId.HasValue)
            {
                var userCart = await _dataContext.carts
                    .Include(x => x.CartItems)
                    .SingleOrDefaultAsync(c => c.userID == userId.Value);
                return userCart;              
            }

            return null;
        }

        public async Task<Cart> DeleteItem(int id)
        {
            var userCart = await GetUserCart(); 

            if (userCart != null)
            {
                var itemToDelete = userCart.CartItems.FirstOrDefault(x => x.id == id);

                if (itemToDelete != null)
                {
                    _dataContext.cartItems.Remove(itemToDelete);
                    await _dataContext.SaveChangesAsync(); 
                }
            }

            return userCart; 
        }

    

    }
}

