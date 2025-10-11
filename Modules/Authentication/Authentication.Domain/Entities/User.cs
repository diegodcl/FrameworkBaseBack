using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Authentication.Domain.Entities
{
    public class User : IdentityUser<Guid>
    {
        public Guid PersonId { get; set; }
        public Guid CustomerId { get; set; }
    }

}