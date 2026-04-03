using TonnUr.Domain.Users;
using TonnUr.Application.Common;

namespace TonnUr.Application.Users.Commands.EnsureUserRegistered;

public record EnsureUserRegisteredCommand(String ExternalId, String Email, String Username) : ICommand<UserId>;