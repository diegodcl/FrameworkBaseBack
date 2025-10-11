using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Organization.Domain.Enums;

namespace Organization.Application.Dto
{
    public record PersonDto
    {
        public PersonType PersonType { get; init; }
        public string Name { get; init; }
        public string? Email { get; init; }
        public string? PhoneNumber { get; init; }
        public string? Reg1 { get; init; }
        public string? LabelReg1 { get; init; }
        public string? Reg2 { get; init; }
        public string? LabelReg2 { get; init; }
        public string? Reg3 { get; init; }
        public string? LabelReg3 { get; init; }
        public string? Reg4 { get; init; }
        public string? LabelReg4 { get; init; }
        public string? Reg5 { get; init; }
        public string? LabelReg5 { get; init; }
        public DateTime? DateOfBirth { get; init; }
    }
}