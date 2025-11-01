using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Organization.Application.Data;
using Organization.Application.Dto;
using Organization.Domain.Entities;
using Organization.Application.Services.Interfaces;
using Organization.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Organization.Application.Services
{
    public class PersonService : IPersonService
    {
        private IOrganizationDbContext _organizationContext { get; set; }

        public PersonService(IOrganizationDbContext organizationContext)
        {
            _organizationContext = organizationContext;
        }

        public async Task<PersonDto> CreateAsync(PersonDto personDto)
        {
            if (personDto is null)
            {
                throw new ArgumentNullException(nameof(personDto));
            }

            ValidatePersonInput(personDto);

            var person = BuildPersonEntity(personDto);

            await _organizationContext.Persons.AddAsync(person);
            await _organizationContext.SaveChangesAsync();

            return MapToDto(person);
        }

        public async Task<IEnumerable<PersonDto>> SearchAsync(Guid? customerId, string? term, int maxResults = 50)
        {
            if (maxResults <= 0)
            {
                maxResults = 50;
            }

            var query = _organizationContext.Persons.AsNoTracking();

            if (customerId.HasValue && customerId.Value != Guid.Empty)
            {
                var id = customerId.Value;
                query = query.Where(person => _organizationContext.Customers.Any(customer => customer.Person.Id == person.Id && customer.Id == id));
            }

            if (!string.IsNullOrWhiteSpace(term))
            {
                var trimmed = term.Trim();
                query = query.Where(person =>
                    EF.Functions.ILike(person.Name, $"%{trimmed}%") ||
                    (!string.IsNullOrEmpty(person.Email) && EF.Functions.ILike(person.Email, $"%{trimmed}%")));
            }

            var persons = await query
                .OrderBy(person => person.Name)
                .Take(maxResults)
                .ToListAsync();

            return persons.Select(MapToDto);
        }

        private static void ValidatePersonInput(PersonDto personDto)
        {
            if (string.IsNullOrWhiteSpace(personDto.Name))
            {
                throw new ArgumentException("Person name is required.", nameof(personDto));
            }

            if (string.IsNullOrWhiteSpace(personDto.Email))
            {
                throw new ArgumentException("Person email is required.", nameof(personDto));
            }

            if (string.IsNullOrWhiteSpace(personDto.PhoneNumber))
            {
                throw new ArgumentException("Person phone number is required.", nameof(personDto));
            }
        }

        private static Person BuildPersonEntity(PersonDto personDto)
        {
            return personDto.PersonType switch
            {
                PersonType.LegalPerson => LegalPerson.Create(
                    personDto.Name,
                    personDto.Email ?? string.Empty,
                    personDto.PhoneNumber ?? string.Empty,
                    personDto.DateOfBirth,
                    personDto.Reg1,
                    personDto.Reg2,
                    personDto.Reg3,
                    personDto.Reg4,
                    personDto.Reg5),
                _ => NaturalPerson.Create(
                    personDto.Name,
                    personDto.Email ?? string.Empty,
                    personDto.PhoneNumber ?? string.Empty,
                    personDto.DateOfBirth,
                    personDto.Reg1,
                    personDto.Reg2,
                    personDto.Reg3,
                    personDto.Reg4,
                    personDto.Reg5)
            };
        }

        private static PersonDto MapToDto(Person person)
        {
            var personType = person switch
            {
                LegalPerson => PersonType.LegalPerson,
                _ => PersonType.NaturalPerson
            };

            return new PersonDto
            {
                Id = person.Id,
                PersonType = personType,
                Name = person.Name,
                Email = person.Email,
                PhoneNumber = person.PhoneNumber,
                Reg1 = person.Reg1,
                LabelReg1 = person.LabelReg1,
                Reg2 = person.Reg2,
                LabelReg2 = person.LabelReg2,
                Reg3 = person.Reg3,
                LabelReg3 = person.LabelReg3,
                Reg4 = person.Reg4,
                LabelReg4 = person.LabelReg4,
                Reg5 = person.Reg5,
                LabelReg5 = person.LabelReg5,
                DateOfBirth = person.DateOfBirth
            };
        }
    }
}