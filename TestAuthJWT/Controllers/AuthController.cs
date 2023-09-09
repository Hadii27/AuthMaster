using AuthMaster.Dtos;
using AuthMaster.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TestAuthJWT.Model;
using TestAuthJWT.Services;

namespace TestAuthJWT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly CartServices _cart;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly UserService _userService;
        public AuthController(IAuthService authService, CartServices cart,UserManager<ApplicationUser> userManager, UserService userService)
        {
            _authService = authService;
            _cart = cart;
            _userManager = userManager;
            _userService = userService;
            
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _authService.RegisterAsync(model);
            if (!result.isAuthenticated)
                return BadRequest(result.Message);
            return Ok(result);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> GetTokenAsync([FromBody] TokenRequestModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.GetTokenAsync(model);
            if (!result.isAuthenticated)
                return BadRequest(result.Message);
            Guid userId = result.userId;
            var blocks = await _userService.GetAllBlackList();
            var blockedUser = blocks.FirstOrDefault(block => block.userId == userId.ToString());
            if (blockedUser is not null)
            {
                return BadRequest($"You have been blocked and the block id is {blocks}.");
            }
            return Ok(result);
        }

        [HttpPost("AddRole")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> AddRoleAsync([FromBody] AddRoleModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.AddRoleAsync(model);
            if (!string.IsNullOrEmpty(result))
                return BadRequest(result);
            return Ok(model);
        }

        [HttpPut("UpdateUser")]
        [Authorize]
        public async Task<IActionResult> UpdateUserAsync([FromBody] UserDto dto)
        {
            var currentUser = _cart.GetCurrentUserId();
            var user = await _userManager.FindByIdAsync(currentUser.ToString());
            if (currentUser.ToString() != user.Id)
            {
                return BadRequest("You can't modify this user");
            }

            if (string.IsNullOrEmpty(dto.Password) || string.IsNullOrEmpty(dto.UserName))
            {
                return BadRequest("Username and password Can't be Empty!.");
            }

            var UsernameValidator = _userManager.UserValidators.FirstOrDefault();
            var passwordValidator = _userManager.PasswordValidators.FirstOrDefault();

            if (passwordValidator != null)
            {
                var validationResult = await passwordValidator.ValidateAsync(_userManager, user, dto.Password);

                if (!validationResult.Succeeded)
                    return BadRequest("Invalid password. Password must meet the password policy.");

            }

            if (UsernameValidator != null)
            {
                var validationResult = await UsernameValidator.ValidateAsync(_userManager, user);

                if (!validationResult.Succeeded)
                    return BadRequest("Invalid Username. Username must meet the password policy.");
            }

            user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, dto.Password);
            user.UserName = dto.UserName;
            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest();
            }
            return Ok(result);

        }

    }
}
