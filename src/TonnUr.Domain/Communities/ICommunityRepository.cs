namespace TonnUr.Domain.Communities;

public interface ICommunityRepository
{
    Task<Community?> GetByIdAsync(CommunityId id, CancellationToken ct = default);
    Task AddAsync(Community community, CancellationToken ct = default);
}
