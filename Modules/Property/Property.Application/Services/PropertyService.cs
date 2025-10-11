using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Property.Application.Dto;
using Property.Application.Services.Interfaces;
using Property.Application.Data;
using Property.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Organization.Shared.Interfaces;

namespace Property.Application.Services
{
    public class PropertyService : IPropertyService
    {
        private IPropertyDbContext _propertyContext { get; set; }
        private ICustomer _customer;
        private bool IsAdmin => string.Equals(_customer?.Alias, "admin", StringComparison.OrdinalIgnoreCase);

        public PropertyService(IPropertyDbContext propertyContext, ICustomer customer)
        {
            _propertyContext = propertyContext;
            _customer = customer;
        }

        private IQueryable<Property.Domain.Entities.Property> ApplyTenantFilter(IQueryable<Property.Domain.Entities.Property> query)
        {
            return IsAdmin ? query : query.Where(p => p.CustomerId == _customer.Id);
        }

        public async Task<PropertyDto> CreateAsync(PropertyDto propertyDto)
        {

            Blueprint blueprint = new Blueprint();
            if (propertyDto.Blueprint != null)
            {
                blueprint.Name = propertyDto.Blueprint.Name ?? string.Empty;
            }

            // Ensure Address is never null
            var address = propertyDto.Address ?? new Core.Domain.ValueObjects.Address
            {
                Line1 = string.Empty,
                City = string.Empty,
                State = string.Empty,
                Country = string.Empty,
                PostCode = string.Empty
            };

            Property.Domain.Entities.Property property = new Property.Domain.Entities.Property(
                Guid.NewGuid(),
                propertyDto.Name,
                propertyDto.Email,
                address,
                _customer.Id,
                propertyDto.PhoneNumber,
                propertyDto.Owner,
                propertyDto.Blueprint != null ? blueprint : null
            );

            _propertyContext.Properties.Add(property);
            await _propertyContext.SaveChangesAsync();

            return propertyDto;
        }

        public async Task<PropertyDto?> UpdateAsync(Guid id, PropertyDto propertyDto)
        {
            var property = await ApplyTenantFilter(_propertyContext.Properties.Include(p => p.Blueprint)).FirstOrDefaultAsync(p => p.Id == id);
            if (property == null)
                return null;

            property.Name = propertyDto.Name;
            property.Email = propertyDto.Email;
            property.PhoneNumber = propertyDto.PhoneNumber;
            property.Owner = propertyDto.Owner;
            property.CustomerId = IsAdmin ? (propertyDto.CustomerId ?? property.CustomerId) : _customer.Id;

            // Update or create Blueprint
            if (propertyDto.Blueprint != null)
            {
                if (property.Blueprint == null)
                {
                    property.Blueprint = new Blueprint
                    {
                        Name = propertyDto.Blueprint.Name ?? string.Empty
                    };
                }
                else
                {
                    property.Blueprint.Name = propertyDto.Blueprint.Name ?? string.Empty;
                }
            }
            else
            {
                property.Blueprint = null; // Remove blueprint if not provided
            }

            // Ensure Address is never null
            var address = propertyDto.Address ?? new Core.Domain.ValueObjects.Address
            {
                Line1 = string.Empty,
                City = string.Empty,
                State = string.Empty,
                Country = string.Empty,
                PostCode = string.Empty
            };
            property.Address = address;

            _propertyContext.Properties.Update(property);
            await _propertyContext.SaveChangesAsync();

            return propertyDto;
        }

        public Task<PropertyDto?> GetByIdAsync(Guid id)
        {
            return ApplyTenantFilter(_propertyContext.Properties)
                .Where(p => p.Id == id)
                .Include(p => p.Blueprint)
                .Select(p => new PropertyDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    PhoneNumber = p.PhoneNumber,
                    Email = p.Email,
                    Address = p.Address,
                    Owner = p.Owner,
                    Blueprint = p.Blueprint != null ? new Property.Application.Dto.BlueprintDto
                    {
                        Id = p.Blueprint.Id,
                        Name = p.Blueprint.Name
                    } : null,
                    CustomerId = p.CustomerId
                })
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<PropertyDto>> GetAllAsync()
        {
            Console.WriteLine($"Tenant ID: {_customer.Id}");

            var properties = await ApplyTenantFilter(_propertyContext.Properties)
                .ToListAsync();
            return properties.Select(p => new PropertyDto
            {
                Id = p.Id,
                Name = p.Name,
                PhoneNumber = p.PhoneNumber,
                Email = p.Email,
                Address = p.Address,
                Owner = p.Owner,
                // Blueprint = p.Blueprint,
                // ClientId = p.ClientId
            });
        }
        
        public async Task<bool> DeleteAsync(Guid id)
        {
            var property = await ApplyTenantFilter(_propertyContext.Properties).FirstOrDefaultAsync(p => p.Id == id);
            if (property == null)
                return false;

            _propertyContext.Properties.Remove(property);
            await _propertyContext.SaveChangesAsync();
            return true;
        }
    }
}