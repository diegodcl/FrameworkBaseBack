using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Organization.Domain.Entities;

namespace Organization.Application.Data
{
    public interface IOrganizationDbContext
    {
        DbSet<Person> Persons { get; }
        DbSet<Customer> Customers { get; }
        DbSet<Address> Addresses { get; }
        
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}