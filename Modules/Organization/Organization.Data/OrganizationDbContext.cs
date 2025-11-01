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
        public DbSet<Render> Renders { get; set; }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Manager> Managers { get; set; }
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

            // Configure Owner relationships
            modelBuilder.Entity<Owner>()
                .ToTable("Owners")
                .HasOne(o => o.Person)
                .WithMany() // Person can have multiple ownership relationships
                .HasForeignKey(o => o.PersonId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete of Person when Owner is deleted

            // Configure ownership history constraints
            modelBuilder.Entity<Owner>()
                .Property(o => o.StartDate)
                .IsRequired();

            modelBuilder.Entity<Owner>()
                .Property(o => o.EndDate)
                .IsRequired(false);

            // Ensure EndDate is after StartDate when both are set
            modelBuilder.Entity<Owner>()
                .ToTable(tb => tb.HasCheckConstraint(
                    "CK_Owner_EndDate_After_StartDate",
                    "\"EndDate\" IS NULL OR \"EndDate\" > \"StartDate\""
                ));

            // Configure indexes for performance
            modelBuilder.Entity<Owner>()
                .HasIndex(o => o.PersonId)
                .HasDatabaseName("IX_Owner_PersonId");

            modelBuilder.Entity<Owner>()
                .HasIndex(o => o.UnitId)
                .HasDatabaseName("IX_Owner_UnitId");

            // Composite index for ownership history queries
            modelBuilder.Entity<Owner>()
                .HasIndex(o => new { o.UnitId, o.StartDate })
                .HasDatabaseName("IX_Owner_UnitId_StartDate");

            // Index for finding current owners (EndDate IS NULL)
            modelBuilder.Entity<Owner>()
                .HasIndex(o => new { o.UnitId, o.EndDate })
                .HasDatabaseName("IX_Owner_UnitId_EndDate")
                .HasFilter("\"EndDate\" IS NULL");

        }
    }
}
