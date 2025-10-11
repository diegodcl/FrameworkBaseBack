using Microsoft.EntityFrameworkCore;
using Organization.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Organization.Domain.Enums;
using Organization.Application.Data;
using Organization.Shared.Interfaces;


namespace Organization.Data
{

    public class OrganizationDbContext : DbContext, IOrganizationDbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Address> Addresses { get; set; }

        public ICustomer _customer { get; set; }


        public OrganizationDbContext(DbContextOptions<OrganizationDbContext> options)
            : base(options)
        {
        }

        public OrganizationDbContext(DbContextOptions<OrganizationDbContext> options, ICustomer customer)
        : base(options) {_customer = customer; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Person>()
                .ToTable("Persons")
                .HasDiscriminator<PersonType>("PersonType")
                .HasValue<NaturalPerson>(PersonType.NaturalPerson)
                .HasValue<LegalPerson>(PersonType.LegalPerson);

            modelBuilder.Entity<Customer>().ToTable("Customers");


        }
    }
}
