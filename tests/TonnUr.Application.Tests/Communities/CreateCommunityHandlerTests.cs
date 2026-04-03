using NSubstitute;
using TonnUr.Application.Abstractions;
using TonnUr.Application.Communities.Commands.CreateCommunity;
using TonnUr.Domain.Communities;

namespace TonnUr.Application.Tests.Communities;

public class CreateCommunityHandlerTests
{
    private readonly ICommunityRepository _communityRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUser _currentUser;
    private readonly CreateCommunityHandler _handler;
    
    public CreateCommunityHandlerTests()
    {
        _communityRepository = Substitute.For<ICommunityRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _currentUser = Substitute.For<ICurrentUser>();

        _handler = new CreateCommunityHandler(
            _communityRepository,
            _unitOfWork,
            _currentUser);
    }
}