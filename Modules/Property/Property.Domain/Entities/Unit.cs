using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Domain.Entities;

namespace Property.Domain.Entities
{
    public class Unit : Base
    {
        public string Identification { get; set; }
        public string? Column { get; set; }
        public decimal? Size { get; set; }
        public bool? IsAvailable { get; set; }

        public IList<Guid>? OwnerId { get; set; }

        public IList<Guid>? ResidentId { get; set; }

        public Blueprint? Blueprint { get; set; }

    }
}