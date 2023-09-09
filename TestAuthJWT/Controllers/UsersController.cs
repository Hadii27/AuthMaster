using AuthMaster.Dtos;
using AuthMaster.Model;
using AuthMaster.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestAuthJWT.Model;

namespace AuthMaster.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;

        public UsersController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<List<GetUsersDto>> GetAll()
        {
            var users = await _userService.GetAllUsers();
            return users;
        }

        [HttpGet("GetOrders")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetOrders(Guid id)
        {
            var orders = await _userService.GetOrders(id);
            if (orders == null)
                return NotFound("No orders found for the user.");
            return Ok(orders);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var user = await _userService.Delete(id);
            return Ok($"{user.UserName} Deleted succesfully ");
        }
    }


}
