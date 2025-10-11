using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Property.Application.Dto;
using Property.Application.Services.Interfaces;
using Property.Domain.Entities;
using Property.Application.Data;
using Microsoft.EntityFrameworkCore;

namespace Property.Application.Services
{
    public class AreaTypeService : IAreaTypeService
    {
        private IPropertyDbContext _propertyContext { get; set; }

        public AreaTypeService(IPropertyDbContext propertyContext)
        {
            _propertyContext = propertyContext;
        }

        public async Task<AreaTypeDto> CreateAsync(AreaTypeDto areaTypeDto)
        {
            // Implementation for creating an AreaType
            // This is a placeholder; actual implementation will depend on the data access layer and business logic.
            AreaType areaType = AreaType.Create(areaTypeDto.Name);

            await _propertyContext.AreaTypes.AddAsync(areaType);
            await _propertyContext.SaveChangesAsync();

            return areaTypeDto;
        }

        public async Task<AreaTypeDto> GetByIdAsync(Guid id)
        {
            var areaType = await _propertyContext.AreaTypes.FindAsync(id);
            return areaType != null ? new AreaTypeDto { Id = areaType.Id, Name = areaType.Name } : null;
        }
        
        public async Task<IEnumerable<AreaTypeDto>> GetAllAsync()
        {
            var areaTypes = await _propertyContext.AreaTypes.ToListAsync();
            return areaTypes.Select(at => new AreaTypeDto { Id = at.Id, Name = at.Name });
        }
    }
}