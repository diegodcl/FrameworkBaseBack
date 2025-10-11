using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Property.Domain.Entities;

namespace Property.Application.Data
{
    public interface IPropertyDbContext
    {
        public DbSet<Property.Domain.Entities.Property> Properties { get; set; }
        public DbSet<Blueprint> Blueprints { get; set; }
        public DbSet<Area> Areas { get; set; }
        public DbSet<AreaType> AreaTypes { get; set; }
        public DbSet<Structure> Structures { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}