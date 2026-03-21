using System.Security.Claims;

namespace TonnUr.Domain.Users;

public record ExternalId(string Value)
{
    public static ExternalId From(ClaimsPrincipal principal) =>
        new(principal.FindFirst("sub")?.Value
            ?? throw new InvalidOperationException("sub claim missing"));
}
