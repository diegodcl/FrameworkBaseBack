using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Domain.Entities;

namespace Organization.Domain.Entities
{
    public class Supplier : Base
    {
        public Person Person { get; set; }
    }
}