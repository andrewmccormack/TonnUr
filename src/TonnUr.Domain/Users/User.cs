using TonnUr.Domain.Common;

namespace TonnUr.Domain.Users;

public class User : AggregateRoot
{
    public UserId Id { get; private set; }
    public ExternalId ExternalId { get; private set; }  // the link to Keycloak
    public string Email { get; private set; }
    public string Username { get; private set; }
    
    public UserStatus Status { get; private set; }
    
    public DateTimeOffset RegisteredAt { get; private set; }
    
    
    public static User Register(ExternalId keycloakId, string email, string username)
    {
        var user = new User
        {
            Id = UserId.New(),
            ExternalId = keycloakId,
            Email = email,
            Username = username,
            Status = UserStatus.Active,
            RegisteredAt = DateTimeOffset.UtcNow
        };

        user.RaiseDomainEvent(new UserRegisteredEvent(user.Id, email));
        return user;
    }
}