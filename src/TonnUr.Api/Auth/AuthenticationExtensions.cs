using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace TonnUr.Api.Auth;

// src/TonnUr.Api/Auth/AuthenticationExtensions.cs
public static class AuthenticationExtensions
{
    public static IServiceCollection AddKeycloakAuthentication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var authOptions = configuration
                              .GetSection(AuthOptions.SectionName)
                              .Get<AuthOptions>()
                          ?? throw new InvalidOperationException("Auth configuration is missing");

        services.Configure<AuthOptions>(
            configuration.GetSection(AuthOptions.SectionName));

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = authOptions.Authority;
                options.Audience = authOptions.Audience;
                options.MapInboundClaims = false; // important — see below

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    NameClaimType = "preferred_username", // Keycloak specific
                    RoleClaimType = "roles"               // Keycloak specific
                };

                // Allow HTTP for local dev — remove in production
                options.RequireHttpsMetadata = false;
            });

        services.AddAuthorization(options =>
        {
            options.AddPolicy("ApiUser", policy =>
                policy.RequireAuthenticatedUser());

            options.AddPolicy("ApiAdmin", policy =>
                policy.RequireAuthenticatedUser()
                    .RequireRole("admin"));
        });

        return services;
    }
}