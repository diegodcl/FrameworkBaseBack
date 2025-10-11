using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Organization.Application.Dto;


namespace Organization.Application.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<CustomerDto> CreateAsync(CustomerDto customerDto);

        Task<CustomerDto?> GetByIdAsync(Guid id);

        Task<IEnumerable<CustomerDto>> GetAllAsync();

        // Task<CustomerDto?> UpdateAsync(Guid id, CustomerDto customerDto);

        // Task<bool> DeleteAsync(Guid id);

        Task<CustomerDto?> GetByAliasAsync(string alias);
    }
}