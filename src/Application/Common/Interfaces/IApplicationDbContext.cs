using EventBridge.Domain.Entities;

namespace EventBridge.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    public DbSet<Event> Events { get; }
    DbSet<Participant> Participants { get; }
    DbSet<EventParticipant> EventParticipants { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
