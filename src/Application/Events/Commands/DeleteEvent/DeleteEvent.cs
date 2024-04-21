using EventBridge.Application.Common.Interfaces;
using EventBridge.Domain.Entities;
using EventBridge.Domain.Events;

namespace EventBridge.Application.Events.Commands.DeleteEvent;

public record DeleteEventCommand(int Id) : IRequest
{
}

public class DeleteEventCommandValidator : AbstractValidator<DeleteEventCommand>
{
}

public class DeleteEventCommandHandler : IRequestHandler<DeleteEventCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteEventCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteEventCommand request, CancellationToken cancellationToken)
    {
        Event? entity = await _context.Events
            .FindAsync([request.Id], cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        _context.Events.Remove(entity);

        entity.AddDomainEvent(new EventDeletedEvent(entity));

        await _context.SaveChangesAsync(cancellationToken);
    }
}
