using System.Security.Claims;
using TonnUr.Application.Abstractions;
using TonnUr.Domain.Users;

namespace TonnUr.Api.Auth;

public sealed class CurrentUser(IHttpContextAccessor accessor, IUserRepository userRepository) : ICurrentUser
{
    private ClaimsPrincipal User => accessor.HttpContext?.User
                                    ?? throw new InvalidOperationException("No HTTP context");

    public string ExternalId => User.FindFirstValue("sub")
                        ?? throw new InvalidOperationException("sub claim missing");

    public string Username => User.FindFirstValue("preferred_username")
                              ?? throw new InvalidOperationException("preferred_username claim missing");

    public string Email => User.FindFirstValue("email")
                           ?? throw new InvalidOperationException("email claim missing");

    public bool IsAdmin => User.IsInRole("admin");
    
    public bool IsAuthenticated => User.Identity?.IsAuthenticated ?? false;
    
    private User? _user;
    public async Task<User> GetDomainUserAsync(CancellationToken ct = default)
    {
        if (_user is not null) return _user;

        _user = await userRepository.GetByExternalIdAsync(
                    new ExternalId(ExternalId), ct)
                ?? throw new InvalidOperationException(
                    "Domain user not found — has UserSyncBehaviour run?");

        return _user;
    }
}