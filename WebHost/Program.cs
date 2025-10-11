using Core.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Configuration.SetBasePath(AppContext.BaseDirectory).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
builder.Services.AddOpenApi();
builder.Services.AddSingleton<SoftDeleteInterceptor>();
builder.Services.AddAuthenticationModule(builder.Configuration);
builder.Services.AddOrganizationModule(builder.Configuration);
builder.Services.AddPropertyModule(builder.Configuration);
builder.Services.AddEndpoints();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapOpenApi();
app.MapScalarApiReference();

app.UseOrganizationModule();

// app.UseHttpsRedirection();
app.UseAuthenticationModule();
app.MapEndpoints();
app.MapIdentityApi<User>();

app.Run();  
