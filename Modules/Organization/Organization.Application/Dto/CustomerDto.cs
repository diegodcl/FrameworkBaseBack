using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Organization.Application.Dto;
using Organization.Domain.Enums;

namespace Organization.Application.Dto
{
    public record CustomerDto
    {
        public Guid? Id { get; set; }
        public PersonDto Person { get; set; }
        public string Alias { get; init; }
        public bool IsActive { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}