using TonnUr.Domain.Users;
using TonnUr.Infrastructure.Common;

namespace TonnUr.Infrastructure.Users.Commands.EnsureUserRegistered;

public record EnsureUserRegisteredCommand(String ExternalId, String Email, String Username) : ICommand<UserId>;