using System.Text.RegularExpressions;
using TonnUr.Domain.Common;

namespace TonnUr.Domain.Communities;

public partial record CommunitySlug
{
    public static readonly int MaxLength = 100;

    [GeneratedRegex(@"^[a-z0-9]+(?:-[a-z0-9]+)*$")]
    private static partial Regex ValidPattern();

    public string Value { get; }

    private CommunitySlug(string value) => Value = value;

    public static Result<CommunitySlug> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Result<CommunitySlug>.Failure("Slug cannot be empty");

        if (value.Length > MaxLength)
            return Result<CommunitySlug>.Failure($"Slug cannot exceed {MaxLength} characters");

        if (!ValidPattern().IsMatch(value))
            return Result<CommunitySlug>.Failure("Slug must be lowercase alphanumeric with hyphens only, and cannot start or end with a hyphen");

        return Result<CommunitySlug>.Success(new CommunitySlug(value));
    }

    public override string ToString() => Value;
}
