using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Property.Domain.Entities
{
    public class Blueprint : Base
    {
        [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(200)]
        public string? Description { get; set; }
        public ICollection<Area> Areas { get; set; }
        public ICollection<Structure> Structures { get; set; }
        public Guid PropertyId { get; set; }
        public Domain.Entities.Property Property { get; set; }

    }
}