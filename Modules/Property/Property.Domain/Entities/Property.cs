using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using Core.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Organization.Shared.Interfaces;

namespace Property.Domain.Entities
{
    [Index(nameof(CustomerId))]
    public class Property : Base, ICustomerOwned
    {
        [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(20)]
        public string? PhoneNumber { get; set; }
        public Email Email { get; set; }
        public Address Address { get; set; }
        public IList<Guid>? Owner { get; set; } // Owner correponde à um Person do módulo Organization - Implementar uma validação para garantir que a Person existam
        public Blueprint Blueprint { get; set; }
        public Guid CustomerId { get; set; }

        public Property() { }

        public Property(Guid id, string name, Email email, Address address, Guid? customerId, string? phoneNumber = null, IList<Guid>? owner = null, Blueprint? blueprint = null)
        {
            Id = id;
            Name = name;
            PhoneNumber = phoneNumber;
            Email = email;
            Address = address;
            Owner = owner;
            Blueprint = blueprint;
            CustomerId = customerId ?? Guid.Empty;
        }
    }
}