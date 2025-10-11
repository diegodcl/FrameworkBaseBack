using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Organization.Application.Dto;
using Organization.Application.Services;
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
            app.MapPost("/persons", async ([FromBody] PersonDto personDto, [FromServices] PersonService personService) =>
            {
                var createdPerson = await personService.CreateAsync(personDto);
                return Results.Ok(createdPerson);
            });
        }
    }
}   