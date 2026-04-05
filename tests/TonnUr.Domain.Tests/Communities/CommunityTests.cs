using System;
using FluentAssertions;
using TonnUr.Domain.Communities;
using TonnUr.Domain.Tests.Common;
using TonnUr.Domain.Users;
using Xunit;

namespace TonnUr.Domain.Tests.Communities;

public class CommunityTests
{
    [Fact]
    public void Create_ShouldReturnCommunity_WithActiveStatus()
    {
        var community = CommunityFactory.Create();

        community.Status.Should().Be(CommunityStatus.Active);
    }

    [Fact]
    public void Create_ShouldReturnCommunity_WithDraftVisibility()
    {
        var community = CommunityFactory.Create();

        community.Visibility.Should().Be(CommunityVisibilty.Draft);
    }

    [Fact]
    public void Create_ShouldSetNameAndDescription()
    {
        var community = CommunityFactory.Create(name: "Test Community", description: "A description");

        community.Name.Should().Be("Test Community");
        community.Description.Should().Be("A description");
    }

    [Fact]
    public void Create_ShouldSetSlug()
    {
        var community = CommunityFactory.Create(slug: "test-community");

        community.Slug.Value.Should().Be("test-community");
    }

    [Fact]
    public void Create_ShouldAddOwnerAsMember()
    {
        var ownerId = new UserId(Guid.NewGuid());

        var community = CommunityFactory.Create(ownerId: ownerId);

        community.Members.Should().ContainSingle()
            .Which.Should().Match<CommunityMember>(m =>
                m.UserId == ownerId && m.Role == CommunityRole.Owner);
    }

    [Fact]
    public void Create_ShouldRaiseCommunityCreatedEvent()
    {
        var community = CommunityFactory.Create();

        community.DomainEvents.Should().ContainSingle()
            .Which.Should().BeOfType<CommunityCreatedEvent>();
    }

    [Fact]
    public void Archive_ShouldSetStatusToArchived()
    {
        var community = CommunityFactory.Create();

        var result = community.Archive();

        result.IsSuccess.Should().BeTrue();
        community.Status.Should().Be(CommunityStatus.Archived);
    }

    [Fact]
    public void Archive_WhenAlreadyArchived_ReturnsFailure()
    {
        var community = CommunityFactory.Create();
        community.Archive();

        var result = community.Archive();

        result.IsFailure.Should().BeTrue();
    }
}
