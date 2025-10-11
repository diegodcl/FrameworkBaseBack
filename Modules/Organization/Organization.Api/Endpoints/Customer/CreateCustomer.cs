using System;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Organization.Application.Dto;
using Organization.Application.Services;
using Organization.Application.Services.Interfaces;
using Core.Infrastructure.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Builder;

namespace Organization.Api.Endpoints
{
    public class CreateCustomer : Core.Infrastructure.Http.Endpoint
    {
        public override void MapEndpoints(WebApplication app)
        {
            app.MapPost("organization/createCustomer", async ([FromServices] ICustomerService customerService, [FromBody] CustomerDto customerDto) =>
            {
                var entity = await customerService.CreateAsync(customerDto);
                return Results.Ok();
            }).RequireAuthorization("CustomerAccess");

            app.MapGet("organization/getCustomer/{id}", async ([FromServices] ICustomerService customerService, Guid id) =>
            {
                var entity = await customerService.GetByIdAsync(id);
                return entity is not null ? Results.Ok(entity) : Results.NotFound();
            }).RequireAuthorization("CustomerAccess");

            // app.MapPut("organization/updateCustomer/{id}", async ([FromServices] ICustomerService customerService, Guid id, [FromBody] CustomerDto customerDto) =>
            // {
            //     var entity = await customerService.UpdateAsync(id, customerDto);
            //     return entity is not null ? Results.Ok(entity) : Results.NotFound();
            // });

            // app.MapDelete("organization/deleteCustomer/{id}", async ([FromServices] ICustomerService customerService, Guid id) =>
            // {
            //     var result = await customerService.DeleteAsync(id);
            //     return result ? Results.NoContent() : Results.NotFound();
            // });

            app.MapGet("organization/getAllCustomers", async ([FromServices] ICustomerService customerService) =>
            {
                var entities = await customerService.GetAllAsync();
                return Results.Ok(entities);
            }).RequireAuthorization("CustomerAccess");
        }


    }
}