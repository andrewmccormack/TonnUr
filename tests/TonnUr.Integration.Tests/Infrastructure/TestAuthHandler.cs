using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace TonnUr.Integration.Tests.Infrastructure;

public sealed class TestAuthHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder)
    : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    public const string SchemeName = "TestAuth";
    public const string DefaultUserId = "test-keycloak-id";
    public const string DefaultEmail = "test@tonnur.com";

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var userId = Request.Headers["X-Test-UserId"].FirstOrDefault()
                     ?? DefaultUserId;
        var email = Request.Headers["X-Test-Email"].FirstOrDefault()
                    ?? DefaultEmail;

        var claims = new[]
        {
            new Claim("sub", userId),
            new Claim("email", email),
            new Claim("preferred_username", email)
        };

        var identity = new ClaimsIdentity(claims, SchemeName);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, SchemeName);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}