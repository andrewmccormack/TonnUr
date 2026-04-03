using MediatR;
using TonnUr.Application.Abstractions;
using TonnUr.Domain.Common;
using TonnUr.Domain.Communities;

namespace TonnUr.Application.Communities.Commands.CreateCommunity;

public sealed class CreateCommunityHandler(
    ICommunityRepository communityRepository,
    IUnitOfWork unitOfWork,
    ICurrentUser currentUser)
    : IRequestHandler<CreateCommunityCommand, Result<CommunityId>>
{
    public async Task<Result<CommunityId>> Handle(
        CreateCommunityCommand command,
        CancellationToken ct)
    {
        var user = await currentUser.GetDomainUserAsync(ct);
        var community = Community.Create(command.Name, command.Description, user.Id);
        await communityRepository.AddAsync(community, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return Result<CommunityId>.Success(community.Id);
    }
}
