using EventBridge.Application.Common.Interfaces;
using EventBridge.Domain.Entities;
using EventBridge.Domain.Events;

namespace EventBridge.Application.EventParticipants.Commands.DeleteEventParticipant;

public record DeleteEventParticipantCommand(int EventId, int ParticipantId) : IRequest
{
}

public class DeleteEventParticipantCommandValidator : AbstractValidator<DeleteEventParticipantCommand>
{
}

public class DeleteEventParticipantCommandHandler : IRequestHandler<DeleteEventParticipantCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteEventParticipantCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteEventParticipantCommand request, CancellationToken cancellationToken)
    {
        object[] keys = new object[] { request.EventId, request.ParticipantId };

        EventParticipant? entity = await _context.EventParticipants.FindAsync(keys, cancellationToken);

        if (entity == null)
        {
            throw new KeyNotFoundException(
                $"Entity with EventId {request.EventId} and ParticipantId {request.ParticipantId} not found.");
        }

        _context.EventParticipants.Remove(entity);
        entity.AddDomainEvent(new EventParticipantDeletedEvent(entity));
        await _context.SaveChangesAsync(cancellationToken);
    }
}
