using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Property.Domain.Entities
{
    public class Structure : Base
    {
        [MaxLength(50)]
        public string Name { get; set; }
        public double? Size { get; set; }
        public string? Description { get; set; }
        public ICollection<Area> Areas { get; set; }
        public Blueprint Blueprint { get; set; }
    }
}