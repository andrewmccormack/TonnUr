using TonnUr.Domain.Common;

namespace TonnUr.Domain.Users;

public record UserDeactivatedEvent(UserId Id) : DomainEvent;