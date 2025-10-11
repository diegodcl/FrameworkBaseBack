using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Infrastructure.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Builder;
using Property.Application.Services.Interfaces;
using Property.Application.Services;
using Property.Application.Dto;
using Microsoft.AspNetCore.Http;

namespace Property.Api.Endpoints.Areas
{
    public class AreaType : Core.Infrastructure.Http.Endpoint
    {
        public override void MapEndpoints(WebApplication app)
        {
            app.MapPost("/property/areatype", async ([FromBody] AreaTypeDto areaTypeDto, [FromServices] IAreaTypeService areaTypeService) =>
            {
                var areaResult = await areaTypeService.CreateAsync(areaTypeDto);
                return Results.Created($"/areatype/{areaResult.Id}", areaResult);
            });

            app.MapGet("/property/areatype", async ([FromServices] IAreaTypeService areaTypeService) =>
            {
                // var areaResult = await areaTypeService.GetAllAsync();
                var entities = await areaTypeService.GetAllAsync();
                return Results.Ok(entities);

                // return areaResult is not null ? Results.Ok(areaResult) : Results.NotFound();
            });
        }
    }
}