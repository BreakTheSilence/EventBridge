using EventBridge.Application.Common.Interfaces;
using EventBridge.Domain.Entities;
using EventBridge.Domain.Enums;
using EventBridge.Domain.Events;

namespace EventBridge.Application.EventParticipants.Commands.CreateEventExisingParticipant;

public record CreateEventExisingParticipantCommand : IRequest<int>
{
    public int EventId { get; set; }
    public int ParticipantId { get; set; }
    public int PaymentMethod { get; set; }
    public int ParticipationCount { get; set; }
    public string? AdditionalInfo { get; set; }
}

public class CreateEventExisingParticipantCommandValidator : AbstractValidator<CreateEventExisingParticipantCommand>
{
    public CreateEventExisingParticipantCommandValidator()
    {
    }
}

public class CreateEventExisingParticipantCommandHandler : IRequestHandler<CreateEventExisingParticipantCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateEventExisingParticipantCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateEventExisingParticipantCommand request, CancellationToken cancellationToken)
    {
        var check = await _context.EventParticipants.FirstOrDefaultAsync(ep => ep.EventId == request.EventId 
                                                                               && ep.ParticipantId == request.ParticipantId, cancellationToken);
        if (check is not null) {
            return check.Id; // Check if this combination already exists
        }

        if (!await _context.Participants.AnyAsync(p => p.Id.Equals(request.ParticipantId), cancellationToken) 
            || !await _context.Events.AnyAsync(e => e.Id.Equals(request.EventId), cancellationToken))
        {
            return -1;
        }
        
        var eventParticipant = new EventParticipant()
        {
            EventId = request.EventId,
            ParticipantId = request.ParticipantId,
            PaymentMethod = (PaymentMethod)request.PaymentMethod,
            ParticipantsCount = request.ParticipationCount,
            AdditionalInfo = request.AdditionalInfo
        };
        
        eventParticipant.AddDomainEvent(new EventParticipantCreatedEvent(eventParticipant));

        _context.EventParticipants.Add(eventParticipant);

        await _context.SaveChangesAsync(cancellationToken);

        return eventParticipant.Id;
    }
}
