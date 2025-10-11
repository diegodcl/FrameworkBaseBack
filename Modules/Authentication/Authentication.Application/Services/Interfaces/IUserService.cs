using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Authentication.Domain.Entities;

namespace Authentication.Application.Services.Interfaces
{
    public interface IUserService
    {
        Task<User?> AuthenticateUserAsync(string email, string password);
    }
}