using System;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Property.Application.Dto;
using Property.Application.Services;
using Property.Application.Services.Interfaces;
using Core.Infrastructure.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Builder;

namespace Property.Api.Endpoints
{
    public class CreateCustomer : Core.Infrastructure.Http.Endpoint
    {
        public override void MapEndpoints(WebApplication app)
        {
            app.MapPost("property/createProperty", async ([FromServices] IPropertyService propertyService, [FromBody] PropertyDto propertyDto) =>
            {
                if (propertyDto == null)
                    throw new ArgumentNullException(nameof(propertyDto));
                    
                var entity = await propertyService.CreateAsync(propertyDto);
                return Results.Ok(entity);
            }).RequireAuthorization("CustomerAccess");

            app.MapGet("property/getProperty/{id}", async ([FromServices] IPropertyService propertyService, Guid id) =>
            {
                var entity = await propertyService.GetByIdAsync(id);
                return entity is not null ? Results.Ok(entity) : Results.NotFound();
            }).RequireAuthorization("CustomerAccess");

            app.MapPut("property/updateProperty/{id}", async ([FromServices] IPropertyService propertyService, Guid id, [FromBody] PropertyDto propertyDto) =>
            {
                var entity = await propertyService.UpdateAsync(id, propertyDto);
                return entity is not null ? Results.Ok(entity) : Results.NotFound();
            }).RequireAuthorization("CustomerAccess");

            app.MapDelete("property/deleteProperty/{id}", async ([FromServices] IPropertyService propertyService, Guid id) =>
            {
                var result = await propertyService.DeleteAsync(id);
                return result ? Results.NoContent() : Results.NotFound();
            }).RequireAuthorization("CustomerAccess");

            app.MapGet("property/getAllProperties", async ([FromServices] IPropertyService propertyService) =>
            {
                var entities = await propertyService.GetAllAsync();
                return Results.Ok(entities);
            }).RequireAuthorization("CustomerAccess");
        }


    }
}