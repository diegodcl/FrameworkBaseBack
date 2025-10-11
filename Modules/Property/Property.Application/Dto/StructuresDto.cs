using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Property.Application.Dto
{
    public record StructureDto
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public ICollection<AreaDto> Areas { get; set; }
        public ICollection<StructureDto> Structures { get; set; }
        public Guid PropertyId { get; set; }
    }
}