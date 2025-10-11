using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Domain.Entities;
using Property.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Property.Domain.Entities
{
    [Index(nameof(BlueprintId))]
    public class Area : Base
    {
        [MaxLength(100)]
        public string Name { get; set; }
        public double? Size { get; set; }
        public AreaType AreaType { get; set; }
        [MaxLength(200)]
        public string? Description { get; set; }
        public ICollection<Area> Areas { get; set; }
        public Guid BlueprintId { get; set; }
        public Blueprint Blueprint { get; set; }
        public ICollection<Structure> Structures { get; set; }
        public UnitType UnitType { get; set; }
    }
}