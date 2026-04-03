using System;
using FluentAssertions;
using TonnUr.Domain.Communities;
using TonnUr.Domain.Users;
using Xunit;

namespace TonnUr.Domain.Tests.Communities;

public class CommunityTests
{
    [Fact]
    public void Create_ShouldReturnCommunity_WithActiveStatus()
    {
        var ownerId = new UserId(Guid.NewGuid());
        var name = "Test Community";
        var  description = "This is a test community";
        // Act
        var community = Community.Create(name, description, ownerId);

        // Assert
        community.Status.Should().Be(CommunityStatus.Active);
        community.Name.Should().Be(name);
        community.Description.Should().Be(description);
        community.OwnerId.Should().Be(ownerId);
    }
    
    [Fact]
    public void Create_ShouldRaise_OrderCreatedEvent()
    {
        var ownerId = new UserId(Guid.NewGuid());
        var name = "Test Community";
        var  description = "This is a test community";
        
        var community = Community.Create(name, description, ownerId);

        community.DomainEvents.Should().ContainSingle()
            .Which.Should().BeOfType<CommunityCreatedEvent>();
    }
}