using EventBridge.Domain.Entities;

namespace EventBridge.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<TodoList> TodoLists { get; }

    DbSet<TodoItem> TodoItems { get; }
    public DbSet<Event> Events { get; }
    DbSet<Participant> Participants { get; }
    DbSet<EventParticipant> EventParticipants { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
