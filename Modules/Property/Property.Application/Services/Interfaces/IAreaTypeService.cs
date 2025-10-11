using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Property.Application.Dto;

namespace Property.Application.Services.Interfaces
{
    public interface IAreaTypeService
    {
        Task<AreaTypeDto> CreateAsync(AreaTypeDto areaTypeDto);
        Task<IEnumerable<AreaTypeDto>> GetAllAsync();
    }
}