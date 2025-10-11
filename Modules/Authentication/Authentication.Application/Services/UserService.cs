using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Authentication.Application.Data;
using Authentication.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Authentication.Application.Services.Interfaces;

namespace Authentication.Application.Services
{
    public class UserService : IUserService
    {
        private IAuthenticationDbContext _authContext { get; set; }
        private UserManager<User> _userManager { get; set; }

        public UserService(IAuthenticationDbContext authContext, UserManager<User> userManager)
        {
            _authContext = authContext;
            _userManager = userManager;
        }

        // public async Task<List<User>> GetAllUsersAsync()
        // {
        //     return await _userManager.Users.ToListAsync();
        // }

        // public async Task<User> GetUserByIdAsync(Guid id)
        // {
        //     return await _userManager.FindByIdAsync(id.ToString());
        // }

        // public Task<List<User>> GetAllAsync()
        // {
        //     throw new NotImplementedException();
        // }

        public async Task<User?> AuthenticateUserAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return null;

            var result = await _userManager.CheckPasswordAsync(user, password);
            if (!result) return null;

            return user;
        }
    }
}