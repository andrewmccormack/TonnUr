namespace TonnUr.Domain.Communities;

public interface ISlugUniquenessChecker
{
    Task<bool> IsUniqueAsync(CommunitySlug slug, CancellationToken ct = default);
}
