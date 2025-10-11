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
using Authentication.Application.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Authentication.Api.Endpoints
{
    public class UserEndpoints : Core.Infrastructure.Http.Endpoint
    {
        public override void MapEndpoints(WebApplication app)
        {
            app.MapGet("auth/users", async ([FromServices] UserManager<User> userManager) =>
            {
                var users = await userManager.Users.ToListAsync();
                var response = new List<UserResponseDto>();

                foreach (var user in users)
                {
                    var roles = await userManager.GetRolesAsync(user);
                    response.Add(ToResponse(user, roles));
                }

                return Results.Ok(response);
            }).RequireAuthorization();

            app.MapGet("auth/users/{id:guid}", async ([FromServices] UserManager<User> userManager, Guid id) =>
            {
                var user = await userManager.FindByIdAsync(id.ToString());
                if (user == null)
                {
                    return Results.NotFound();
                }

                var roles = await userManager.GetRolesAsync(user);
                return Results.Ok(ToResponse(user, roles));
            }).RequireAuthorization();

            app.MapPut("auth/users/{id:guid}", async ([FromServices] UserManager<User> userManager, Guid id, [FromBody] UserUpdateDto updateDto) =>
            {
                var user = await userManager.FindByIdAsync(id.ToString());
                if (user == null)
                {
                    return Results.NotFound();
                }

                if (!string.IsNullOrWhiteSpace(updateDto.Email))
                {
                    user.Email = updateDto.Email;
                    user.NormalizedEmail = updateDto.Email.ToUpperInvariant();
                }

                if (!string.IsNullOrWhiteSpace(updateDto.UserName))
                {
                    user.UserName = updateDto.UserName;
                    user.NormalizedUserName = updateDto.UserName.ToUpperInvariant();
                }

                if (updateDto.EmailConfirmed.HasValue)
                {
                    user.EmailConfirmed = updateDto.EmailConfirmed.Value;
                }

                var updateResult = await userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    return Results.BadRequest(updateResult.Errors);
                }

                if (updateDto.Roles is not null)
                {
                    var currentRoles = await userManager.GetRolesAsync(user);
                    var rolesToRemove = currentRoles.Except(updateDto.Roles).ToArray();
                    var rolesToAdd = updateDto.Roles.Except(currentRoles).ToArray();

                    if (rolesToRemove.Length > 0)
                    {
                        await userManager.RemoveFromRolesAsync(user, rolesToRemove);
                    }

                    if (rolesToAdd.Length > 0)
                    {
                        await userManager.AddToRolesAsync(user, rolesToAdd);
                    }
                }

                var roles = await userManager.GetRolesAsync(user);
                return Results.Ok(ToResponse(user, roles));
            }).RequireAuthorization();

            app.MapDelete("auth/users/{id:guid}", async ([FromServices] UserManager<User> userManager, Guid id) =>
            {
                var user = await userManager.FindByIdAsync(id.ToString());
                if (user == null)
                {
                    return Results.NotFound();
                }

                var result = await userManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    return Results.BadRequest(result.Errors);
                }

                return Results.NoContent();
            }).RequireAuthorization();

            // Legacy routes retained for backward compatibility
            app.MapDelete("auth/deleteuser/{id}", async ([FromServices] UserManager<User> userManager, [FromRoute] string id) =>
            {
                var user = await userManager.FindByIdAsync(id);
                if (user == null) return Results.NotFound();
                var result = await userManager.DeleteAsync(user);
                return result.Succeeded ? Results.Ok() : Results.BadRequest(result.Errors);
            }).RequireAuthorization();

            app.MapPut("auth/updateuser/{id}", async ([FromServices] UserManager<User> userManager, [FromRoute] string id, [FromBody] string newEmail) =>
            {
                var user = await userManager.FindByIdAsync(id);
                if (user == null) return Results.NotFound();
                if (string.IsNullOrWhiteSpace(newEmail))
                {
                    return Results.BadRequest("Email is required.");
                }

                user.Email = newEmail;
                user.NormalizedEmail = newEmail.ToUpperInvariant();
                var result = await userManager.UpdateAsync(user);
                return result.Succeeded ? Results.Ok(ToResponse(user, await userManager.GetRolesAsync(user))) : Results.BadRequest(result.Errors);
            }).RequireAuthorization();

            // Authenticate User (built-in endpoints handle this)

            // Create Role (removed to avoid conflicts)

            // Create Policy (for demo, just a placeholder)
            app.MapPost("auth/createpolicy", (string policyName) =>
            {
                // In ASP.NET Core, policies are configured in code, not at runtime.
                // You can store policy definitions in DB and load at startup if needed.
                return Results.Ok($"Policy '{policyName}' would be created/configured at startup.");
            });

            static UserResponseDto ToResponse(User user, IEnumerable<string> roles) => new()
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                EmailConfirmed = user.EmailConfirmed,
                CustomerId = user.CustomerId,
                Roles = roles
            };
        }
    }
}