using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Domain.ValueObjects
{
    [ComplexType]
    public record Address
    {
        [MaxLength(100)]
        public required string Line1 { get; set; }
        [MaxLength(100)]
        public string? Line2 { get; set; }
        [MaxLength(100)]
        public required string City { get; set; }
        [MaxLength(100)]
        public required string State { get; set; }
        [MaxLength(50)]
        public required string Country { get; set; }
        [MaxLength(20)]
        public required string PostCode { get; set; }
        [MaxLength(100)]
        public string? Area { get; set; }
    }
}