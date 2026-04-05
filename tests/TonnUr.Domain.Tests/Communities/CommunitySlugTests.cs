using FluentAssertions;
using TonnUr.Domain.Communities;
using Xunit;

namespace TonnUr.Domain.Tests.Communities;

public class CommunitySlugTests
{
    [Theory]
    [InlineData("my-community")]
    [InlineData("dublin-js")]
    [InlineData("abc")]
    [InlineData("community123")]
    [InlineData("123")]
    [InlineData("a1-b2-c3")]
    public void Create_WithValidSlug_ReturnsSuccess(string value)
    {
        var result = CommunitySlug.Create(value);

        result.IsSuccess.Should().BeTrue();
        result.Value!.Value.Should().Be(value);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithEmptyOrWhitespace_ReturnsFailure(string value)
    {
        var result = CommunitySlug.Create(value);

        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public void Create_ExceedingMaxLength_ReturnsFailure()
    {
        var value = new string('a', CommunitySlug.MaxLength + 1);

        var result = CommunitySlug.Create(value);

        result.IsFailure.Should().BeTrue();
    }

    [Theory]
    [InlineData("My-Community")]     // uppercase
    [InlineData("my community")]     // spaces
    [InlineData("-my-community")]    // leading hyphen
    [InlineData("my-community-")]    // trailing hyphen
    [InlineData("my--community")]    // consecutive hyphens
    [InlineData("my_community")]     // underscores
    [InlineData("my.community")]     // dots
    public void Create_WithInvalidFormat_ReturnsFailure(string value)
    {
        var result = CommunitySlug.Create(value);

        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public void ToString_ReturnsSlugValue()
    {
        var result = CommunitySlug.Create("my-community");

        result.Value!.ToString().Should().Be("my-community");
    }
}
