using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Infrastructure.Data.Interfaces
{
    public interface ISoftDeletable
    {
        bool IsDeleted { get; set; }

        DateTime? DeletedOnUtc { get; set; }
    }
}