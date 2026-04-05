using TonnUr.Domain.Common;
using TonnUr.Domain.Users;

namespace TonnUr.Domain.Communities;

public class Community : AggregateRoot
{
    public CommunityId Id { get; private set; }
    public string Name { get; private set; }
    public CommunitySlug Slug { get; private set; }
    public string? Description { get; private set; }
    public CommunityStatus Status { get; private set; }
    public CommunityVisibilty Visibility { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }

    public List<CommunityMember> Members { get; private set; } = [];


    public static Community Create(string name, CommunitySlug slug, string? description, UserId ownerId)
    {
        var community = new Community
        {
            Id = CommunityId.New(),
            Name = name,
            Slug = slug,
            Description = description,
            Status = CommunityStatus.Active,
            Visibility = CommunityVisibilty.Draft,
            Members = [new CommunityMember(ownerId, CommunityRole.Owner, DateTimeOffset.UtcNow)],
            CreatedAt = DateTimeOffset.UtcNow
        };

        community.RaiseDomainEvent(new CommunityCreatedEvent(community.Id, name, ownerId));
        return community;
    }

    public Result Archive()
    {
        if (Status == CommunityStatus.Archived)
            return Result.Failure("Community is already archived");

        Status = CommunityStatus.Archived;
        return Result.Success();
    }
}
