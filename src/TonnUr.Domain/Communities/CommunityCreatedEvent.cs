using TonnUr.Domain.Common;
using TonnUr.Domain.Users;

namespace TonnUr.Domain.Communities;

public record CommunityCreatedEvent(CommunityId CommunityId, string Name, UserId OwnerId) : DomainEvent;
