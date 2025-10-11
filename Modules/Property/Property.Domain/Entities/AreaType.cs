using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using Core.Infrastructure.Data.Interfaces;

namespace Property.Domain.Entities
{
    public class AreaType : Base
    {
        [MaxLength(100)]
        public string Name { get; set; }
        public static AreaType Create(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name cannot be null or empty.", nameof(name));
            }

            return new AreaType
            {
                Name = name
            };
        }
    }
}