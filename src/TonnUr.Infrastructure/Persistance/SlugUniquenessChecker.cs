using Microsoft.EntityFrameworkCore;
using TonnUr.Domain.Communities;

namespace TonnUr.Infrastructure.Persistance;

public class SlugUniquenessChecker(AppDbContext dbContext) : ISlugUniquenessChecker
{
    public async Task<bool> IsUniqueAsync(CommunitySlug slug, CancellationToken ct = default)
    {
        return !await dbContext.Communities
            .AnyAsync(c => c.Slug == slug, ct);
    }
}
