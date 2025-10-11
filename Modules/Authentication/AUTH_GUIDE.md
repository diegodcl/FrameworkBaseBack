# Authentication and Authorization Guide for Condominio

## Overview
This guide explains how authentication and authorization are implemented in the Condominio project, including the main files involved, the flow of user identity, and how to secure endpoints. It is intended for developers maintaining or extending the authentication system.

---

## 1. Key Concepts
- **Authentication:** Verifies the identity of a user (login, registration, etc.).
- **Authorization:** Determines what an authenticated user is allowed to do (roles, policies, claims).
- **ASP.NET Core Identity:** Used for user and role management, password hashing, and security features.
- **Entity Framework Core:** Used for persisting user and role data.
- **Custom Claims:** Used to inject additional information (e.g., CustomerId) into the user principal for multitenancy.

---

## 2. Main Files Involved

| File/Folder                                                                 | Purpose                                                                                 |
|-----------------------------------------------------------------------------|-----------------------------------------------------------------------------------------|
| `backend/Modules/Authentication/Authentication.Api/Authentication.cs`       | Registers Identity, roles, claims principal factory, and authorization policies.         |
| `backend/Modules/Authentication/Authentication.Data/AuthenticationDbContext.cs` | EF Core DbContext for Identity (users, roles, claims, etc.).                            |
| `backend/Modules/Authentication/Authentication.Domain/Entities/User.cs`      | User entity, inherits from `IdentityUser`.                                              |
| `backend/Modules/Authentication/Authentication.Api/Authentication.cs` (CustomUserClaimsPrincipalFactory) | Injects custom claims (e.g., CustomerId) into the user principal.                       |
| `backend/WebHost/Program.cs`                                                | Calls authentication/authorization registration during app startup.                      |
| `backend/WebHost/appsettings.json`                                          | Stores configuration for Identity, JWT, etc.                                            |

---

## 3. Service Registration Flow

### a. Identity and Roles
- Registered in `Authentication.Api/Authentication.cs`:
  ```csharp
  services.AddIdentity<User, IdentityRole>()
      .AddEntityFrameworkStores<AuthenticationDbContext>()
      .AddDefaultTokenProviders();
  services.AddIdentityApiEndpoints<User>();
  services.AddRoles<IdentityRole>();
  ```
- This ensures all required services for user and role management are available.

### b. Claims Principal Factory
- Registered in `Authentication.Api/Authentication.cs`:
  ```csharp
  services.AddScoped<IUserClaimsPrincipalFactory<User>, CustomUserClaimsPrincipalFactory>();
  ```
- The factory (defined in `Authentication.Api/Authentication.cs`) injects custom claims (e.g., CustomerId) for multitenancy.

### c. Authorization Policies
- Defined in `Authentication.Api/Authentication.cs`:
  ```csharp
  options.AddPolicy("CustomerAccess", policy =>
      policy.RequireClaim("CustomerId"));
  options.AddPolicy("AdminRole", policy =>
      policy.RequireRole("Admin"));
  ```
- Used to restrict access to endpoints based on claims or roles.

---

## 4. How Authentication Works

1. **User Registration/Login:**
   - Handled by built-in Identity API endpoints (enabled via `AddIdentityApiEndpoints<User>()`).
   - Users are persisted in the `AuthenticationDbContext` database.

2. **Claims Injection:**
   - After login, `CustomUserClaimsPrincipalFactory` adds custom claims (e.g., CustomerId) to the user principal.

3. **User Identity:**
   - Accessed via `User` property in controllers or endpoints.
   - Example: `var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;`

---

## 5. How Authorization Works

1. **Role-Based Authorization:**
   - Assign roles (e.g., Admin) to users.
   - Restrict endpoints using `[Authorize(Roles = "Admin")]`.

2. **Policy-Based Authorization:**
   - Use custom policies (e.g., CustomerAccess) to restrict endpoints.
   - Example: `[Authorize(Policy = "CustomerAccess")]`

3. **Claims-Based Authorization:**
   - Restrict access based on specific claims (e.g., CustomerId).

---

## 6. Securing Endpoints

### a. Controller Example
```csharp
[Authorize(Policy = "CustomerAccess")]
[ApiController]
[Route("api/[controller]")]
public class MyController : ControllerBase
{
    // ...
}
```

### b. Minimal API Example
```csharp
app.MapGet("/secure-endpoint", [Authorize(Roles = "Admin")] (ClaimsPrincipal user) =>
{
    // ...
});
```

---

## 7. Checking User Identity in Code

- **Get User Id:**
  ```csharp
  var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
  ```
- **Get Custom Claim (e.g., CustomerId):**
  ```csharp
  var customerId = User.FindFirst("CustomerId")?.Value;
  ```
- **Check Role:**
  ```csharp
  if (User.IsInRole("Admin")) { /* ... */ }
  ```

---

## 8. Adding/Modifying Authorization Policies
- Edit `Authentication.Api/Authentication.cs` in the `AddAuthorization` section.
- Example:
  ```csharp
  options.AddPolicy("ManagerOnly", policy =>
      policy.RequireRole("Manager"));
  ```

---

## 9. Troubleshooting
- **RoleManager/IRoleStore errors:** Ensure `.AddRoles<IdentityRole>()` is present in Identity registration.
- **Duplicate claims principal factory:** Only register `CustomUserClaimsPrincipalFactory` once.
- **Access denied:** Check that the user has the required role/claim and that the policy is correctly applied.

---

## 10. References
- [ASP.NET Core Identity Docs](https://learn.microsoft.com/aspnet/core/security/authentication/identity)
- [Authorization in ASP.NET Core](https://learn.microsoft.com/aspnet/core/security/authorization/introduction)

---

_Last updated: October 11, 2025_
