namespace TonnUr.Domain.Communities;

public record CommunityId(Guid Value)
{
    public static CommunityId New() => new(Guid.NewGuid());
}
