using MediatR;
using TonnUr.Application.Abstractions;
using TonnUr.Domain.Common;
using TonnUr.Domain.Communities;

namespace TonnUr.Application.Communities.Commands.CreateCommunity;

public sealed class CreateCommunityHandler(
    ICommunityRepository communityRepository,
    ISlugUniquenessChecker slugUniquenessChecker,
    ISlugGenerator slugGenerator,
    IUnitOfWork unitOfWork,
    ICurrentUser currentUser)
    : IRequestHandler<CreateCommunityCommand, Result<CommunityId>>
{
    public async Task<Result<CommunityId>> Handle(
        CreateCommunityCommand command,
        CancellationToken ct)
    {
        var rawSlug = command.Slug ?? slugGenerator.Generate(command.Name);

        var slugResult = CommunitySlug.Create(rawSlug);
        if (slugResult.IsFailure)
            return Result<CommunityId>.Failure(slugResult.Error!);

        var isUnique = await slugUniquenessChecker.IsUniqueAsync(slugResult.Value!, ct);
        if (!isUnique)
            return Result<CommunityId>.Failure("A community with this slug already exists");

        var user = await currentUser.GetDomainUserAsync(ct);
        var community = Community.Create(command.Name, slugResult.Value!, command.Description, user.Id);

        await communityRepository.AddAsync(community, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return Result<CommunityId>.Success(community.Id);
    }
}
