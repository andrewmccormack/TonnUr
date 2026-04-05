using System;
using TonnUr.Domain.Communities;
using TonnUr.Domain.Users;

namespace TonnUr.Domain.Tests.Common;

public static class CommunityFactory
{
    public static Community Create(
        string name = "Test Community",
        string slug = "test-community",
        string? description = "A test community description",
        UserId? ownerId = null)
    {
        var communitySlug = CommunitySlug.Create(slug).Value!;
        return Community.Create(name, communitySlug, description, ownerId ?? new UserId(Guid.NewGuid()));
    }
}
