using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Organization.Application.Data;
using Organization.Application.Dto;
using Organization.Domain.Entities;
using Organization.Application.Services.Interfaces;

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
            var person = NaturalPerson.Create(
                personDto.Name,
                personDto.Email,
                personDto.PhoneNumber,
                personDto.DateOfBirth,
                personDto.Reg1,
                personDto.Reg2,
                personDto.Reg3,
                personDto.Reg4,
                personDto.Reg5
            );

            await _organizationContext.Persons.AddAsync(person);
            await _organizationContext.SaveChangesAsync();

            return personDto;
        }
    }
}