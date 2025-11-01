using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Domain.Entities;

namespace Organization.Domain.Entities
{
    public class Owner : Base
    {
        public Guid PersonId { get; set; }
        public Person Person { get; set; }
        public Guid UnitId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        
    }
}