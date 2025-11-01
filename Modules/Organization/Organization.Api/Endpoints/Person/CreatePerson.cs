using System;
using Organization.Application.Dto;
using Organization.Application.Services.Interfaces;
using Core.Infrastructure.Http.Interface;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace Organization.Api.Endpoints.Person
{
    public class CreatePerson : IEndpoint
    {
        public void MapEndpoints(WebApplication app)
        {
            app.MapPost("/persons", async ([FromBody] PersonDto personDto, [FromServices] IPersonService personService) =>
            {
                var createdPerson = await personService.CreateAsync(personDto);
                return Results.Ok(createdPerson);
            });

            app.MapGet("/persons", async ([FromQuery] Guid? customerId, [FromQuery] string? search, [FromServices] IPersonService personService) =>
            {
                var persons = await personService.SearchAsync(customerId, search);
                return Results.Ok(persons);
            });
        }
    }
}   