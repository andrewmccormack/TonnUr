using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using TonnUr.Domain.Common;

namespace TonnUr.Infrastructure.Persistance;

public class AppDbContext(DbContextOptions<AppDbContext> options, IPublisher publisher) : DbContext(options)
{
    public override async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        var domainEvents = CollectDomainEvents();
        var result = await base.SaveChangesAsync(ct);
        await DispatchDomainEvents(domainEvents, ct);
        return result;
    }

    private List<DomainEvent> CollectDomainEvents()
    {
        return ChangeTracker
            .Entries<AggregateRoot>()
            .Select(e => e.Entity)
            .SelectMany(e =>
            {
                var events = e.DomainEvents.ToList();
                e.ClearDomainEvents();
                return events;
            })
            .ToList();
    }
    
    private async Task DispatchDomainEvents(List<DomainEvent> domainEvents, CancellationToken ct)
    {
        foreach (var domainEvent in domainEvents) 
            await publisher.Publish(domainEvent, ct);
    }
}