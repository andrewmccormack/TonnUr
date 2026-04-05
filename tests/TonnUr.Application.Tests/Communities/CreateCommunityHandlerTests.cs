using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using TonnUr.Application.Abstractions;
using TonnUr.Application.Communities.Commands.CreateCommunity;
using TonnUr.Domain.Communities;
using TonnUr.Domain.Users;
using Xunit;

namespace TonnUr.Application.Tests.Communities;

public class CreateCommunityHandlerTests
{
    private readonly ICommunityRepository _communityRepository;
    private readonly ISlugUniquenessChecker _slugUniquenessChecker;
    private readonly ISlugGenerator _slugGenerator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUser _currentUser;
    private readonly CreateCommunityHandler _handler;

    public CreateCommunityHandlerTests()
    {
        _communityRepository = Substitute.For<ICommunityRepository>();
        _slugUniquenessChecker = Substitute.For<ISlugUniquenessChecker>();
        _slugGenerator = Substitute.For<ISlugGenerator>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _currentUser = Substitute.For<ICurrentUser>();

        var user = User.Register(new ExternalId("ext-123"), "test@example.com", "testuser");
        _currentUser.GetDomainUserAsync(Arg.Any<CancellationToken>()).Returns(user);

        _handler = new CreateCommunityHandler(
            _communityRepository,
            _slugUniquenessChecker,
            _slugGenerator,
            _unitOfWork,
            _currentUser);
    }

    [Fact]
    public async Task Handle_WithNoSlug_GeneratesSlugFromName()
    {
        _slugGenerator.Generate("My Community").Returns("my-community");
        _slugUniquenessChecker.IsUniqueAsync(Arg.Any<CommunitySlug>(), Arg.Any<CancellationToken>()).Returns(true);

        var command = new CreateCommunityCommand("My Community", null);

        await _handler.Handle(command, CancellationToken.None);

        _slugGenerator.Received(1).Generate("My Community");
    }

    [Fact]
    public async Task Handle_WithSlugProvided_SkipsGeneration()
    {
        _slugUniquenessChecker.IsUniqueAsync(Arg.Any<CommunitySlug>(), Arg.Any<CancellationToken>()).Returns(true);

        var command = new CreateCommunityCommand("My Community", null, "my-community");

        await _handler.Handle(command, CancellationToken.None);

        _slugGenerator.DidNotReceiveWithAnyArgs().Generate(default!);
    }

    [Fact]
    public async Task Handle_WhenSlugIsUnique_ReturnsCommunityId()
    {
        _slugGenerator.Generate(Arg.Any<string>()).Returns("my-community");
        _slugUniquenessChecker.IsUniqueAsync(Arg.Any<CommunitySlug>(), Arg.Any<CancellationToken>()).Returns(true);

        var command = new CreateCommunityCommand("My Community", null);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
    }

    [Fact]
    public async Task Handle_WhenSlugIsNotUnique_ReturnsFailure()
    {
        _slugGenerator.Generate(Arg.Any<string>()).Returns("my-community");
        _slugUniquenessChecker.IsUniqueAsync(Arg.Any<CommunitySlug>(), Arg.Any<CancellationToken>()).Returns(false);

        var command = new CreateCommunityCommand("My Community", null);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_WhenSlugIsNotUnique_DoesNotSaveCommunity()
    {
        _slugGenerator.Generate(Arg.Any<string>()).Returns("my-community");
        _slugUniquenessChecker.IsUniqueAsync(Arg.Any<CommunitySlug>(), Arg.Any<CancellationToken>()).Returns(false);

        var command = new CreateCommunityCommand("My Community", null);

        await _handler.Handle(command, CancellationToken.None);

        await _communityRepository.DidNotReceiveWithAnyArgs().AddAsync(default!, default);
        await _unitOfWork.DidNotReceiveWithAnyArgs().SaveChangesAsync(default);
    }

    [Fact]
    public async Task Handle_WhenSlugIsInvalid_ReturnsFailure()
    {
        // Generator returns an invalid slug (e.g. infrastructure bug)
        _slugGenerator.Generate(Arg.Any<string>()).Returns("INVALID SLUG!");

        var command = new CreateCommunityCommand("My Community", null);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
    }
}
