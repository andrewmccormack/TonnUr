using TonnUr.Api;
using TonnUr.Api.Auth;
using TonnUr.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddKeycloakAuthentication(builder.Configuration)
    .AddApi()
    .AddApplication()
    .AddInfrastructure(builder.Configuration)
    .AddAuthorization();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.Run();

