using System;
using Core.Infrastructure.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Authentication.Application.Dto;
using Microsoft.AspNetCore.Mvc;
using Authentication.Domain.Entities;
using Organization.Shared.Interfaces;
using System.Threading.Tasks;

namespace Authentication.Api.Endpoints
{
  public class CreatePage : Core.Infrastructure.Http.Endpoint
  {
    public override void MapEndpoints(WebApplication app)
    {
      app.MapPost("auth/createuser", async (
        [FromServices]UserManager<User> userManager,
        [FromServices] ICustomer customerContext,
        [FromBody]UserDataDto? userDataDto) =>
      {
          return await HandleCreateUser(userManager, customerContext, userDataDto);
      });

      app.MapPost("auth/users", async (
        [FromServices]UserManager<User> userManager,
        [FromServices] ICustomer customerContext,
        [FromBody]UserDataDto? userDataDto) =>
      {
          return await HandleCreateUser(userManager, customerContext, userDataDto);
      });

      static async Task<IResult> HandleCreateUser(
        UserManager<User> userManager,
        ICustomer customerContext,
        UserDataDto? userDataDto)
      {
        if (userDataDto == null)
        {
          return Results.BadRequest("Invalid user data.");
        }
        if (string.IsNullOrWhiteSpace(userDataDto.Email) || string.IsNullOrWhiteSpace(userDataDto.Password))
        {
          return Results.BadRequest("Email and password are required.");
        }

        if (userDataDto.PersonId is null || userDataDto.PersonId == Guid.Empty)
        {
          return Results.BadRequest("A valid person identifier is required to create a user.");
        }

        var clientId = userDataDto.CustomerId ?? Guid.Empty;
        if (clientId == Guid.Empty)
        {
          clientId = customerContext.Id;
        }
        else if (customerContext.Id != Guid.Empty && customerContext.Id != clientId)
        {
          return Results.BadRequest("The selected customer does not match the current tenant context.");
        }

        if (clientId == Guid.Empty)
        {
          return Results.BadRequest("Unable to resolve the customer context for this request.");
        }
        var user = new User
        {
          UserName = userDataDto.UserName ?? userDataDto.Email,
          Email = userDataDto.Email,
          PersonId = userDataDto.PersonId.Value,
          CustomerId = clientId
        };
        var result = await userManager.CreateAsync(user, userDataDto.Password);
        return result.Succeeded ? Results.Ok(user) : Results.BadRequest(result.Errors);
      }
    }
  }
}