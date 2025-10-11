using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authentication.Application.Dto
{
    public record UserResponseDto
    {
        public Guid Id { get; init; }
        public string? Email { get; init; }
        public string? UserName { get; init; }
        public bool EmailConfirmed { get; init; }
        public Guid CustomerId { get; init; }
        public IEnumerable<string> Roles { get; init; } = Enumerable.Empty<string>();
    }
}
