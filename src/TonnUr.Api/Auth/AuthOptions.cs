namespace TonnUr.Api.Auth;

public sealed class AuthOptions
{
    public const string SectionName = "Auth";
    
    public string Authority { get; init; } = string.Empty;
    public string Audience { get; init; } = string.Empty;
}