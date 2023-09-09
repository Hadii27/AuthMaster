using AuthMaster.Dtos;
using AuthMaster.Model;
using AuthMaster.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestAuthJWT.Data;
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

        [HttpPost("Block")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> BlockUser(string UserID)
        {
            var user = await _userService.GetByID(UserID);


            if (user == null)
            {
                return NotFound("User not found.");
            }

            var blocks = await _userService.GetAllBlackList();

            // Convert UserID to a string for comparison

            // Check if the user is already in the blacklist
            if (blocks.Any(block => block.userId == UserID))
            {
                return BadRequest("This user is already blocked.");
            }

            var blockedUser = await _userService.Block(UserID);

            return Ok(blockedUser);
        }


    }


}
