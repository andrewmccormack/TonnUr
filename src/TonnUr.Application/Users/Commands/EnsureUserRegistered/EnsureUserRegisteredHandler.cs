using MediatR;
using TonnUr.Application.Abstractions;
using TonnUr.Domain.Common;
using TonnUr.Domain.Users;

namespace TonnUr.Infrastructure.Users.Commands.EnsureUserRegistered;

public sealed class EnsureUserRegisteredHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<EnsureUserRegisteredCommand, Result<UserId>>
{
    public async Task<Result<UserId>> Handle(
        EnsureUserRegisteredCommand command,
        CancellationToken ct)
    {
        var keycloakId = new ExternalId(command.ExternalId);

        // Idempotent — just return existing user if already registered
        var existing = await userRepository.GetByExternalIdAsync(keycloakId, ct);
        if (existing is not null)
            return Result<UserId>.Success(existing.Id);

        var user = User.Register(keycloakId, command.Email, command.Username);

        await userRepository.AddAsync(user, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return Result<UserId>.Success(user.Id);
    }
}