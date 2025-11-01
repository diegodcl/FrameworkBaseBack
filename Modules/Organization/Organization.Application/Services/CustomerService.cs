using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Organization.Application.Data;
using Organization.Domain.Entities;
using Organization.Application.Dto;
using Organization.Domain.Enums;
using Organization.Application.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Organization.Domain.Enums;
using Organization.Shared.Interfaces;

namespace Organization.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private IOrganizationDbContext _organizationContext { get; set; }

        private ICustomer _customer;

        public CustomerService(IOrganizationDbContext organizationContext, ICustomer customer)
        {
            _organizationContext = organizationContext;
            _customer = customer;
        }

        public async Task<CustomerDto> CreateAsync(CustomerDto customerDto)
        {

            Customer customer = Customer.CreateCustomer(
                customerDto.Person.Name,
                customerDto.Person.Email,
                customerDto.Person.PhoneNumber,
                customerDto.Person.DateOfBirth,
                customerDto.Person.Reg1,
                customerDto.Person.Reg2,
                customerDto.Person.Reg3,
                customerDto.Person.Reg4,
                customerDto.Person.Reg5,
                customerDto.Alias,
                customerDto.Person.PersonType
            );

            await _organizationContext.Customers.AddAsync(customer);
            await _organizationContext.SaveChangesAsync();

            return customerDto;
        }

        public async Task<CustomerDto?> GetByIdAsync(Guid id)
        {

            var customer = await _organizationContext.Customers.FindAsync(id);

            PersonDto personDto = new PersonDto
            {
                Id = customer.Person.Id,
                Name = customer.Person.Name,
                Email = customer.Person.Email,
                PhoneNumber = customer.Person.PhoneNumber,
                DateOfBirth = customer.Person.DateOfBirth,
                Reg1 = customer.Person.Reg1,
                Reg2 = customer.Person.Reg2,
                Reg3 = customer.Person.Reg3,
                Reg4 = customer.Person.Reg4,
                Reg5 = customer.Person.Reg5,
                PersonType = customer.Person is NaturalPerson ? PersonType.NaturalPerson : PersonType.LegalPerson
            };


            CustomerDto customerDto = new CustomerDto
            {
                Id = customer.Id,
                Person = personDto,
                Alias = customer.Alias
            };

            return customer is not null ? customerDto : null;
        }

        public async Task<IEnumerable<CustomerDto>> GetAllAsync()
        {
            Console.WriteLine($"Tenant ID: {_customer.Id}");
            var customers = await _organizationContext.Customers
                .Include(c => c.Person)
                .ToListAsync();
            return customers.Select(c =>
            {
                PersonDto personDto = new PersonDto
                {
                    Id = c.Person.Id,
                    Name = c.Person.Name,
                    Email = c.Person.Email,
                    PhoneNumber = c.Person.PhoneNumber,
                    DateOfBirth = c.Person.DateOfBirth,
                    Reg1 = c.Person.Reg1,
                    Reg2 = c.Person.Reg2,
                    Reg3 = c.Person.Reg3,
                    Reg4 = c.Person.Reg4,
                    Reg5 = c.Person.Reg5,
                    PersonType = c.Person is NaturalPerson ? PersonType.NaturalPerson : PersonType.LegalPerson
                };
                return new CustomerDto
                {
                    Id = c.Id,
                    Person = personDto,
                    IsActive = c.IsActive,
                    Alias = c.Alias
                };
            });
        }

        public async Task<CustomerDto?> GetByAliasAsync(string alias)
        {

            var customer = await _organizationContext.Customers.FirstOrDefaultAsync(c => c.Alias == alias);

            if (customer == null)
            {
                return null;
            }

            // PersonDto personDto = new PersonDto
            // {
            //     Name = customer.Person.Name,
            //     Email = customer.Person.Email,
            //     PhoneNumber = customer.Person.PhoneNumber,
            //     DateOfBirth = customer.Person.DateOfBirth,
            //     Reg1 = customer.Person.Reg1,
            //     Reg2 = customer.Person.Reg2,
            //     Reg3 = customer.Person.Reg3,
            //     Reg4 = customer.Person.Reg4,
            //     Reg5 = customer.Person.Reg5,
            //     // PersonType = customer.Person.PersonType
            // };


            CustomerDto customerDto = new CustomerDto
            {
                Id = customer.Id,
                // Person = personDto,
                Alias = customer.Alias
            };

            return customer is not null ? customerDto : null;
        }
    }
}