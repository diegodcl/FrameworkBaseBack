using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authentication.Application.Dto
{
    public record UserUpdateDto
    {
        public string? Email { get; init; }
        public string? UserName { get; init; }
        public bool? EmailConfirmed { get; init; }
        public List<string>? Roles { get; init; } = new List<string>();
    }
}
