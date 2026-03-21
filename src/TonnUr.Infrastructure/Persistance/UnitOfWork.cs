using TonnUr.Application.Abstractions;

namespace TonnUr.Infrastructure.Persistance;

public class UnitOfWork(AppDbContext dbContext) : IUnitOfWork
{
    public Task<int> SaveChangesAsync(CancellationToken ct = default)
        => dbContext.SaveChangesAsync(ct);
    
    public Task BeginTransactionAsync(CancellationToken ct = default) 
        => dbContext.Database.BeginTransactionAsync(ct);

    public Task CommitTransactionAsync(CancellationToken ct = default) 
        => dbContext.Database.CommitTransactionAsync(ct);

    public Task RollbackTransactionAsync(CancellationToken ct = default) 
        => dbContext.Database.RollbackTransactionAsync(ct);
}