using TonnUr.Domain.Communities;
using TonnUr.Application.Common;

namespace TonnUr.Application.Communities.Commands.CreateCommunity;

public record CreateCommunityCommand(string Name, string? Description, string? Slug = null) : ICommand<CommunityId>;
