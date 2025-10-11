using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Authentication.Domain.Entities;

namespace Authentication.Application.Data
{
    public interface IAuthenticationDbContext
    {
        public DbSet<User> Users { get; set; }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}