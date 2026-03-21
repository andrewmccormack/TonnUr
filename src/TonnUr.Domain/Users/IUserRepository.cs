namespace TonnUr.Domain.Users;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(UserId id, CancellationToken ct = default);
    Task<User?> GetByExternalIdAsync(ExternalId keycloakId, CancellationToken ct = default);
    Task AddAsync(User user, CancellationToken ct = default);
}