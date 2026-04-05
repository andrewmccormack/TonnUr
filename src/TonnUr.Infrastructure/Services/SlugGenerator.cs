using System.Text;
using System.Text.RegularExpressions;
using TonnUr.Application.Abstractions;
using TonnUr.Domain.Communities;

namespace TonnUr.Infrastructure.Services;

public partial class SlugGenerator : ISlugGenerator
{
    [GeneratedRegex(@"[^a-z0-9]+")]
    private static partial Regex NonAlphanumeric();

    [GeneratedRegex(@"-{2,}")]
    private static partial Regex ConsecutiveHyphens();

    public string Generate(string name)
    {
        var normalized = name.Normalize(NormalizationForm.FormD);

        var asciiOnly = new string(normalized
            .Where(c => (int)c < 128)
            .ToArray());

        var lowered = asciiOnly.ToLowerInvariant();
        var hyphenated = NonAlphanumeric().Replace(lowered, "-");
        var collapsed = ConsecutiveHyphens().Replace(hyphenated, "-");
        var trimmed = collapsed.Trim('-');

        return trimmed.Length > CommunitySlug.MaxLength
            ? trimmed[..CommunitySlug.MaxLength].TrimEnd('-')
            : trimmed;
    }
}
