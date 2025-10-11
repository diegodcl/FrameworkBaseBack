using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authentication.Application.Dto
{
    public record UserLoginDto
    {
        public string? Email { get; init; }
        public string? Password { get; init; }
    }
}