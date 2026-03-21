using System.Security.Claims;

namespace TonnUr.Api.Auth;

public sealed class CurrentUser(IHttpContextAccessor accessor)
{
    private ClaimsPrincipal User => accessor.HttpContext?.User
                                    ?? throw new InvalidOperationException("No HTTP context");

    public string Id => User.FindFirstValue("sub")
                        ?? throw new InvalidOperationException("sub claim missing");

    public string Username => User.FindFirstValue("preferred_username")
                              ?? throw new InvalidOperationException("preferred_username claim missing");

    public string Email => User.FindFirstValue("email")
                           ?? throw new InvalidOperationException("email claim missing");

    public bool IsAdmin => User.IsInRole("admin");
    
    public bool IsAuthenticated => User.Identity?.IsAuthenticated ?? false;
}