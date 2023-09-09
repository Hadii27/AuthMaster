using AuthMaster.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using TestAuthJWT.Helpers;
using TestAuthJWT.Model;

namespace TestAuthJWT.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JWT _jwt;
        public AuthService(UserManager<ApplicationUser> userManager, IOptions<JWT> jwt, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _jwt = jwt.Value;
            _roleManager = roleManager;
        }
        public async Task<AuthModel> RegisterAsync(RegisterModel model)
        {
            if (await _userManager.FindByEmailAsync(model.Email) is not null)
                return new AuthModel { Message = "Email is already exist!" };
            if (await _userManager.FindByNameAsync(model.Username) is not null)
                return new AuthModel { Message = "Username is already exist!" };

            var user = new ApplicationUser
            {
                UserName = model.Username,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                string errors = string.Empty;
                foreach (var error in result.Errors)
                {
                    errors += $"{error.Description},";
                }
                return new AuthModel { Message = errors };
            }

            await _userManager.AddToRoleAsync(user, "User");
            var jwtSecurityToken = await CreateJwtTokenAsync(user); // Await the token creation method
            var userID = await _userManager.GetUserIdAsync(user);
            Guid? userId = null; // Initialize userId to null

            if (Guid.TryParse(userID, out Guid parsedUserId))
            {
                userId = parsedUserId;
            }

            return new AuthModel
            {
                Email = user.Email,
                ExpireOn = jwtSecurityToken.ValidTo,
                isAuthenticated = true, // Corrected property name
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                Roles = new List<string> { "User" },
                userId = parsedUserId
            };


        }

        public async Task <AuthModel> GetTokenAsync(TokenRequestModel model)
        {
            var authModel = new AuthModel();
            var user = await _userManager.FindByNameAsync(model.Username);
            var userID = await _userManager.GetUserIdAsync(user);
            if (user == null || !await _userManager.CheckPasswordAsync(user,model.Password))
            {
                authModel.Message = "Email Or Password Not Correct!";
                return authModel;
            }

            var jwtSecurityToken = await CreateJwtTokenAsync(user);
            var roleList = await _userManager.GetRolesAsync(user);
            
            authModel.ExpireOn = jwtSecurityToken.ValidTo;
            authModel.isAuthenticated = true;
            authModel.Email = user.Email;
            authModel.Username = user.UserName;
            authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            authModel.Roles = roleList.ToList();
            if (Guid.TryParse(userID, out Guid userId))
            {
                authModel.userId = userId;
            }
            return authModel;
        }
        public async Task<string> AddRoleAsync(AddRoleModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);

            if (user == null || !await _roleManager.RoleExistsAsync(model.RoleName))
            {
                return "Invalid user or role";
            }

            if (await _userManager.IsInRoleAsync(user, model.RoleName))
            {
                return "This user is already assigned to this role";
            }

            var addRoleResult = await _userManager.AddToRoleAsync(user, model.RoleName);
            if (addRoleResult.Succeeded)
            {
                var removeUserRoleResult = await _userManager.RemoveFromRoleAsync(user, "User");
                return string.Empty;
            }
            else
            {
                return "Failed to add role to user";
            }
        }

        private async Task<JwtSecurityToken> CreateJwtTokenAsync(ApplicationUser user)
        {
            var userclaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();
            foreach (var role in roles)
            {
                roleClaims.Add(new Claim("roles", role));
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("UserId", user.Id.ToString()),

            }
            .Union(userclaims)
            .Union(roleClaims);
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey,SecurityAlgorithms.HmacSha256);
            var JwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.Now.AddHours(_jwt.DurationInHoues),
                signingCredentials: signingCredentials
                );
            return JwtSecurityToken;

        }
    }
}
