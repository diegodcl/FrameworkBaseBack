using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Property.Domain.Entities;
using Core.Domain.ValueObjects;

namespace Property.Application.Dto
{
    public record PropertyDto
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string? PhoneNumber { get; set; }
        public Email Email { get; set; }
        public Address? Address { get; set; }
        public IList<Guid>? Owner { get; set; } 
        public BlueprintDto? Blueprint { get; set; }
        public Guid? CustomerId { get; set; }
    }
}