using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Organization.Application.Dto;

namespace Organization.Application.Services.Interfaces
{
    public interface IPersonService
    {
        public Task<PersonDto> CreateAsync(PersonDto personDto);
        public Task<IEnumerable<PersonDto>> SearchAsync(Guid? customerId, string? term, int maxResults = 50);
    }
}