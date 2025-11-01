using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authentication.Application.Dto
{
    public record UserDataDto
    {
        
        public string? Id { get; init; }
        public string? Email { get; init; }
        public string? UserName { get; init; }
        public string? Password { get; init; }
        public bool? EmailConfirmed { get; init; }
        public DateTime? CreatedAt { get; init; }
        public DateTime? LastLoginAt { get; init; }
        public List<string>? Roles { get; init; } = new List<string>();
        public Guid? PersonId { get; init; }
        public Guid? CustomerId { get; init; }
    }
}