using AuthMaster.Dtos;
using AuthMaster.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Server.IIS.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging; // Add this namespace for logging
using System;
using System.Security.Claims;
using TestAuthJWT.Data;
using TestAuthJWT.Migrations;
using TestAuthJWT.Model;

namespace AuthMaster.Services
{
    public class UserService
    {
        private readonly DataContext _dataContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly BlockList _blockList;

        public UserService(DataContext dataContext, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor, BlockList blockList)
        {
            _dataContext = dataContext;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _blockList = blockList;
        }

        //public async Task<ApplicationUser> GetCurrentUserInfoAsync()
        //{
        //    var currentUser = _httpContextAccessor.HttpContext.User;
        //    var userId = currentUser.FindFirstValue(ClaimTypes.NameIdentifier);
        //    if (userId != null)
        //    {
        //        var user = await _userManager.FindByIdAsync(userId);
        //        return user;
        //    }
        //    return null;
        //}

        //public async Task<ApplicationUser> UpdateUserAsync(UserDto dto)
        //{
        //    var currentUser = GetCurrentUserInfoAsync();
        //    var user = await _userManager.FindByIdAsync(currentUser.ToString());
        //    var UsernameValidator = _userManager.UserValidators.FirstOrDefault();
        //    var passwordValidator = _userManager.PasswordValidators.FirstOrDefault();

        //    if (passwordValidator != null)
        //    {
        //        var validationResult = await passwordValidator.ValidateAsync(_userManager, user, dto.Password);
        //    }

        //    if (UsernameValidator != null)
        //    {
        //        var validationResult = await UsernameValidator.ValidateAsync(_userManager, user);             
        //    }

        //    if (user != null)
        //    {
        //        user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, dto.Password);
        //        user.UserName = dto.UserName;
        //        user.FirstName = dto.FirstName;
        //        user.LastName = dto.LastName;

        //        return user;
        //    }

        //    var result = await _userManager.UpdateAsync(user);
        //    return user;

        //}
        public async Task<List<GetUsersDto>> GetAllUsers()
        {
            var user = await _dataContext.Users
                .OrderBy(u => u.UserName)
                .Select(u => new GetUsersDto
                {
                    Email = u.Email,
                    UserName = u.UserName,
                    LastName = u.LastName,
                    FirstName = u.FirstName,
                    PhoneNumber = u.PhoneNumber,
                    UserId = u.Id
                })

                .ToListAsync();
            return user;
        }

        public async Task<IEnumerable<Order>> GetOrders(Guid id)
        {
            var orders = await _dataContext.orders
                .OrderBy(u => u.DateTime)
                .Where(Order => Order.UserId == id)
                .ToListAsync();
            return orders;

        }

        public async Task<ApplicationUser> Delete(Guid ID)
        {
            string IdString = ID.ToString();
            var user = await _dataContext.Users
                .Where(u => u.Id == IdString)
                .SingleOrDefaultAsync();
            var result = _dataContext.Users.Remove(user);
            _dataContext.SaveChanges();
            return user;
        }

        public async Task<ApplicationUser> GetByID(string id)
        {
            string IdString = id.ToString();
            var user = await _dataContext.Users
                .Where(u => u.Id == IdString)
                .SingleOrDefaultAsync();
            return user;
        }


        public async Task<IEnumerable<BlockList>> GetAllBlackList()
        {
            var blackListEntries = await _dataContext.blockLists.ToListAsync();

            return blackListEntries;
        }
        public async Task<BlockList> Block(string ID)
        {
            string IdString = ID.ToString();
            var user = await _dataContext.Users
                .Where(u => u.Id == IdString)
                .SingleOrDefaultAsync();
            
                var BlockUser = new BlockList
                {
                    userId = ID,
                    Blocked = "True",
                    dateTime = DateTime.Now,
                    Reason = "Abuse",
                };

                _dataContext.blockLists.Add(BlockUser);
                await _dataContext.SaveChangesAsync();
                return BlockUser;            
        }

     

    }




}
