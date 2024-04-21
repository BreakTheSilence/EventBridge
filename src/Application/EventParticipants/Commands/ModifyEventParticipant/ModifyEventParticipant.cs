using EventBridge.Application.Common.Interfaces;
using EventBridge.Domain.Entities;
using EventBridge.Domain.Enums;

namespace EventBridge.Application.EventParticipants.Commands.ModifyEventParticipant;

public record ModifyEventParticipantCommand : IRequest
{
    public int EventId { get; set; }
    public int ParticipantId { get; set; }
    public int PaymentMethod { get; set; }
    public int ParticipantsCount { get; set; }
    public string? AdditionalInfo { get; set; }
}

public class ModifyEventParticipantCommandValidator : AbstractValidator<ModifyEventParticipantCommand>
{
}

public class ModifyEventParticipantCommandHandler : IRequestHandler<ModifyEventParticipantCommand>
{
    private readonly IApplicationDbContext _context;

    public ModifyEventParticipantCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(ModifyEventParticipantCommand request, CancellationToken cancellationToken)
    {
        EventParticipant? entity =
            await _context.EventParticipants.FindAsync(request.EventId, request.ParticipantId, cancellationToken);
        if (entity is null)
        {
            throw new KeyNotFoundException(
                $"Entity with EventId {request.EventId} and ParticipantId {request.ParticipantId} not found.");
        }

        Participant? participant = await _context.Participants.FindAsync(request.ParticipantId, cancellationToken);

        if (participant == null)
        {
            throw new KeyNotFoundException($"Participant with Id {request.ParticipantId} not found.");
        }

        entity.PaymentMethod = (PaymentMethod)request.PaymentMethod;
        entity.ParticipantsCount = participant.Type == ParticipantType.Company ? request.ParticipantsCount : 1;
        entity.AdditionalInfo = request.AdditionalInfo;

        await _context.SaveChangesAsync(cancellationToken);
    }
}
