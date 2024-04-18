using EventBridge.Application.Common.Interfaces;
using EventBridge.Domain.Entities;
using EventBridge.Domain.Enums;
using EventBridge.Domain.Events;

namespace EventBridge.Application.EventParticipants.Commands.CreateEventNewParticipant;

public record CreateEventNewParticipantCommand : IRequest<int>
{
    public int EventId { get; set; }

    public int ParticipantType { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Name { get; set; }
    public long IdCode { get; set; }
    
    public int PaymentMethod { get; set; }
    public int ParticipationCount { get; set; }
    public string? AdditionalInfo { get; set; }
}

public class CreateEventNewParticipantCommandValidator : AbstractValidator<CreateEventNewParticipantCommand>
{
    public CreateEventNewParticipantCommandValidator()
    {
    }
}

public class CreateEventNewParticipantCommandHandler : IRequestHandler<CreateEventNewParticipantCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateEventNewParticipantCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateEventNewParticipantCommand request, CancellationToken cancellationToken)
    {
        var participant = await SaveParticipant(request, cancellationToken);

        var eventParticipant = new EventParticipant()
        {
            EventId = request.EventId,
            ParticipantId = participant.Id,
            PaymentMethod = (PaymentMethod)request.PaymentMethod,
            ParticipantsCount = participant.Type == ParticipantType.Company ? request.ParticipationCount : 1,
            AdditionalInfo = request.AdditionalInfo
        };
        
        eventParticipant.AddDomainEvent(new EventParticipantCreatedEvent(eventParticipant));

        _context.EventParticipants.Add(eventParticipant);

        await _context.SaveChangesAsync(cancellationToken);

        return 200;
    }

    private async Task<Participant> SaveParticipant(CreateEventNewParticipantCommand request, CancellationToken cancellationToken)
    {
        var participantType = (ParticipantType)request.ParticipantType;
        var participant = new Participant()
        {
            Type = participantType,
            FirstName = participantType == ParticipantType.Individual ? request.FirstName : null,
            LastName = participantType == ParticipantType.Individual ? request.LastName : null,
            Name = participantType == ParticipantType.Company ? request.Name : null,
            IdCode = request.IdCode
        };
        
        participant.AddDomainEvent(new ParticipantCreatedEvent(participant));

        _context.Participants.Add(participant);

        await _context.SaveChangesAsync(cancellationToken);

        return participant;
    }
}
