using System.Reflection;
using FluentAssertions;
using MediatR;
using NetArchTest.Rules;
using TonnUr.Domain.Communities;
using TonnUr.Application.Communities.Commands.CreateCommunity;
using TonnUr.Infrastructure.Persistance;
using Xunit;

namespace TonnUr.Architecture.Tests;

// tests/TonnUr.Architecture.Tests/LayerDependencyTests.cs
public sealed class LayerDependencyTests
{
    private static readonly Assembly DomainAssembly =
        typeof(Community).Assembly;
    private static readonly Assembly ApplicationAssembly =
        typeof(CreateCommunityCommand).Assembly;
    private static readonly Assembly InfrastructureAssembly =
        typeof(AppDbContext).Assembly;
    private static readonly Assembly ApiAssembly =
        typeof(Program).Assembly;

    [Fact]
    public void Domain_ShouldNot_DependOnApplication()
    {
        Types.InAssembly(DomainAssembly)
            .Should().NotHaveDependencyOn("TonnUr.Application")
            .GetResult().IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Domain_ShouldNot_DependOnInfrastructure()
    {
        Types.InAssembly(DomainAssembly)
            .Should().NotHaveDependencyOn("TonnUr.Infrastructure")
            .GetResult().IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Application_ShouldNot_DependOnInfrastructure()
    {
        Types.InAssembly(ApplicationAssembly)
            .Should().NotHaveDependencyOn("TonnUr.Infrastructure")
            .GetResult().IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Application_ShouldNot_DependOnApi()
    {
        Types.InAssembly(ApplicationAssembly)
            .Should().NotHaveDependencyOn("TonnUr.Api")
            .GetResult().IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Handlers_ShouldBe_Sealed()
    {
        Types.InAssembly(ApplicationAssembly)
            .That().ImplementInterface(typeof(IRequestHandler<,>))
            .Should().BeSealed()
            .GetResult().IsSuccessful.Should().BeTrue();
    }
}