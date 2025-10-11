using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Organization.Shared.Interfaces
{
    public interface ICustomer
    {
        public Guid Id { get; set; }
        public string Alias { get; set; }
    }
}