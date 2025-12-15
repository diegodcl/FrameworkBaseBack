# Definitive Module Authoring Guide (Condominio)

This guide merges current instructions and patterns from existing modules (e.g., Organization, Property) so Copilot can generate new backend modules given a prompt containing `[ModuleName]`, entities, and use cases.

## Architecture Baseline
- Stack: .NET 9, C#, EF Core (Npgsql), Minimal APIs, Clean Architecture per module (Api/Application/Domain/Data).
- Common: `Core` project provides `Core.Domain.Entities.Base`, HTTP `Endpoint` base, DI helpers, and tenant abstractions (`ICustomer`, `ICustomerOwned`).
- Authorization: Use policy `CustomerAccess` on module endpoints unless explicitly open.

## Module Layout
```
backend/Modules/[ModuleName]/
  [ModuleName].Api/
    [ModuleName].cs               // DI + middleware registration
    appsettings.json              // module connection string (same key as Program uses)
    Endpoints/
      [Feature]/[Feature].cs      // minimal API endpoint class per feature
  [ModuleName].Application/
    Dto/                          // response/request DTOs
    Services/
      Interfaces/I[Entity]Service.cs
      [Entity]Service.cs
    Data/I[ModuleName]DbContext.cs
  [ModuleName].Data/
    [ModuleName]DbContext.cs
    [ModuleName]DesignTimeDbContextFactory.cs
    Migrations/
  [ModuleName].Domain/
    Entities/[Entity].cs
    (Enums/ValueObjects as needed)
```
Add the four projects to `Condominio.sln` and reference `Core.csproj` plus the sibling projects as shown in existing modules.

## Domain Layer
- Each entity inherits `Core.Domain.Entities.Base` (Id, CreatedAt, UpdatedAt).
- Multitenancy: any customer-scoped entity **must** implement `ICustomerOwned` and include `[Index(nameof(CustomerId))]`; set `CustomerId` in the constructor/factory and enforce tenant invariants. See `Property.Domain.Entities.Property` for reference.
- Rich model: prefer rich-domain methods/factories over anemic setters. Encapsulate mutations inside the entity (e.g., `UpdateContact`, `AssignOwner`, `ChangeAddress`) and keep invariants there. Expose only the mutations you need; avoid arbitrary public setters where possible.
- Keep a parameterless constructor for EF plus factory/parameterized constructors for invariants.

## Data Layer
- `I[ModuleName]DbContext` exposes `DbSet<T>` for all entities plus `SaveChangesAsync`.
- `[ModuleName]DbContext : DbContext, I[ModuleName]DbContext` registers those sets.
- Design-time factory: derive from `GenericDesignTimeDbContextFactory<[ModuleName]DbContext>` (preferred) or implement `IDesignTimeDbContextFactory` if custom; ensure it reads the module connection string.

## Application Layer
- DTOs:
  - Response DTOs include `Id`, main fields, and timestamps where relevant.
  - Create/Update DTOs exclude `Id` and timestamps.
- Services:
  - Interface `I[Entity]Service` exposes async CRUD operations.
  - Implementation injects `I[ModuleName]DbContext` and, when tenant-aware, `ICustomer`. Pattern from `PropertyService`:
    - `IsAdmin => string.Equals(_customer?.Alias, "admin", StringComparison.OrdinalIgnoreCase)`
    - Apply tenant filter: `IsAdmin ? query : query.Where(e => e.CustomerId == _customer.Id)`
  - Map DTOs <-> entities; set `CreatedAt/UpdatedAt` on create and update.
  - `DeleteAsync` returns `bool` for existence.
    - Application services are defined in the `Services` folder within each module's Application project.
  - Each service is responsible for a specific business capability and interacts with the domain layer to perform operations.
  - The `Data` folder contains repositories and data access code, which interact with the database using Entity Framework Core. The repositories are injected into the application services to perform CRUD operations.

## API Layer (Minimal APIs)
- Each endpoint class inherits `Core.Infrastructure.Http.Endpoint` and overrides `MapEndpoints`.
- Map every user-requested use case as its own minimal-API route (separate handlers per use case/feature), all calling application services from `[ModuleName].Application`.
- Use a route group with authorization:
  ```csharp
  var group = app.MapGroup("/[entity-lower]").RequireAuthorization("CustomerAccess");
  ```
- Standard CRUD mappings (adjust names to your feature):
  ```csharp
  group.MapGet("", async (I[Entity]Service service, CancellationToken ct) => Results.Ok(await service.GetAllAsync(ct)));
  group.MapGet("/{id:guid}", async (I[Entity]Service service, Guid id, CancellationToken ct) =>
      (await service.GetByIdAsync(id, ct)) is { } item ? Results.Ok(item) : Results.NotFound());
  group.MapPost("", async (I[Entity]Service service, [FromBody] Create[Entity]Dto dto, CancellationToken ct) =>
      Results.Created($"/[entity]/{(await service.CreateAsync(dto, ct)).Id}", await service.CreateAsync(dto, ct)));
  group.MapPut("/{id:guid}", async (I[Entity]Service service, Guid id, [FromBody] Update[Entity]Dto dto, CancellationToken ct) =>
      (await service.UpdateAsync(id, dto, ct)) is { } updated ? Results.Ok(updated) : Results.NotFound());
  group.MapDelete("/{id:guid}", async (I[Entity]Service service, Guid id, CancellationToken ct) =>
      await service.DeleteAsync(id, ct) ? Results.NoContent() : Results.NotFound());
  ```
- For uploads or form-data, add `.DisableAntiforgery()` when needed and stream to storage as in Documents module.
- Keep validation simple at endpoint level (e.g., required name) to return 400 before service exceptions.
- Validate primitive inputs at the endpoint (e.g., required strings not empty, Guid parsable, numeric ranges) before calling services; return `Results.BadRequest(...)` on validation failures.

## Module Registration
- In `[ModuleName].cs` (Api project):
  ```csharp
  services.AddDbContext<[ModuleName]DbContext>(opt => opt.UseNpgsql(configuration.GetConnectionString("[ModuleName]Connection")));
  services.AddScoped<I[ModuleName]DbContext, [ModuleName]DbContext>();
  services.AddScoped<I[Entity]Service, [Entity]Service>();
  return services;
  ```
  Add middleware in `Use[ModuleName]Module` only if required (e.g., tenant middleware in Organization).

- In `backend/WebHost/Program.cs` register services and pipeline:
  ```csharp
  builder.Services.Add[ModuleName]Module(builder.Configuration);
  ...
  app.Use[ModuleName]Module(); // only if module defines middleware
  ```

- Add connection string to `backend/WebHost/appsettings.json`:
  ```json
  "[ModuleName]Connection": "Host=localhost;Port=5432;Database=Condominio[ModuleName];Username=postgres;Password=yourpwd"
  ```

## EF Migrations
From `backend/` run:
```
dotnet ef migrations add Initial[ModuleName] -c [ModuleName]DbContext -p Modules/[ModuleName]/[ModuleName].Data/[ModuleName].Data.csproj -s WebHost/WebHost.csproj -o Migrations

dotnet ef database update -c [ModuleName]DbContext -p Modules/[ModuleName]/[ModuleName].Data/[ModuleName].Data.csproj -s WebHost/WebHost.csproj
```

## Coding Conventions
- Follow .NET naming; keep files ASCII.
- Add concise XML docs to public types/methods in Application/Api layers.
- Apply tenant filtering for any `ICustomerOwned` entity.

## Quick Template (replace tokens)
- Entity (Domain): inherit `Base`, optional `ICustomerOwned`.
- DTOs: `[Entity]Dto` (with Id), `Create/Update[Entity]Dto` (no Id).
- Service: map DTOs, set timestamps, filter by tenant.
- Endpoints: Minimal APIs under `/[entity]`, authorized with `CustomerAccess`, CRUD routes.
- DI: register DbContext + services in `[ModuleName].cs`; add connection string; register module in WebHost.

Use Organization module for DI + middleware example, Property module for tenant-filtered service + CRUD endpoints, and Documents module for upload patterns when dealing with files.
