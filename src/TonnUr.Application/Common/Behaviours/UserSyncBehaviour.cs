using MediatR;
using TonnUr.Application.Abstractions;
using TonnUr.Domain.Users;
using TonnUr.Application.Users.Commands.EnsureUserRegistered;

namespace TonnUr.Application.Common.Behaviours;

// src/TonnUr.Application/Common/Behaviours/UserSyncBehaviour.cs
public sealed class UserSyncBehaviour<TRequest, TResponse>(
    IUserRepository userRepository,
    ISender sender,
    ICurrentUser currentUser)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest: ICommand  // only runs for commands, not queries
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken ct)
    {
        if (!currentUser.IsAuthenticated)
            return await next(ct);

        var keycloakId = new ExternalId(currentUser.ExternalId);
        var exists = await userRepository.GetByExternalIdAsync(keycloakId, ct);

        if (exists is null)
            await sender.Send(new EnsureUserRegisteredCommand(
                currentUser.ExternalId,
                currentUser.Email,
                currentUser.Username), ct);

        return await next(ct);
    }
}
