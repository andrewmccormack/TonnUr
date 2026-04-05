using TonnUr.Domain.Users;

namespace TonnUr.Domain.Communities;

public record CommunityMember(UserId UserId, CommunityRole Role, DateTimeOffset JoinedAt)
{
}