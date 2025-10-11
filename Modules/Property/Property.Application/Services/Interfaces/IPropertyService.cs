using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Property.Application.Dto;

namespace Property.Application.Services.Interfaces
{
    public interface IPropertyService
    {
        Task<PropertyDto> CreateAsync(PropertyDto propertyDto);

        Task<PropertyDto?> GetByIdAsync(Guid id);

        Task<IEnumerable<PropertyDto>> GetAllAsync();

        Task<PropertyDto?> UpdateAsync(Guid id, PropertyDto propertyDto);

        Task<bool> DeleteAsync(Guid id);
    }
}

