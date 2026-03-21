using TonnUr.Domain.Users;

namespace TonnUr.Application.Abstractions;

public interface ICurrentUser
{
    string ExternalId { get; }
    string Username { get; }
    string Email { get; }
    bool IsAuthenticated { get; }
    bool IsAdmin { get; }
    Task<User> GetDomainUserAsync(CancellationToken ct = default);
}