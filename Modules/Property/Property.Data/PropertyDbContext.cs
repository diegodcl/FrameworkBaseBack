using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Property.Domain.Entities;
using Property.Application.Data;
using Organization.Shared.Interfaces;
using System.Linq.Expressions;

namespace Property.Data
{
    public class PropertyDbContext : DbContext, IPropertyDbContext
    {
        public DbSet<Property.Domain.Entities.Property> Properties { get; set; }
        public DbSet<Blueprint> Blueprints { get; set; }
        public DbSet<Area> Areas { get; set; }
        public DbSet<AreaType> AreaTypes { get; set; }
        public DbSet<Structure> Structures { get; set; }

        public PropertyDbContext(DbContextOptions<PropertyDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Removed global query filter as it was set at model build time and not per request
            // Filters are now applied in the application layer per request
        }
    }
}