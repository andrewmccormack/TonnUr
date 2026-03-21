using TonnUr.Domain.Common;

namespace TonnUr.Domain.Users;

public record UserRegisteredEvent(UserId UserId, String Email) : DomainEvent;