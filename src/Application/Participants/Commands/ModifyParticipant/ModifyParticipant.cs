using EventBridge.Application.Common.Interfaces;
using EventBridge.Domain.Entities;
using EventBridge.Domain.Enums;

namespace EventBridge.Application.Participants.Commands.ModifyParticipant;

public record ModifyParticipantCommand : IRequest
{
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Name { get; set; }
    public long IdCode { get; set; }
}

public class ModifyParticipantCommandValidator : AbstractValidator<ModifyParticipantCommand>
{
}

public class ModifyParticipantCommandHandler : IRequestHandler<ModifyParticipantCommand>
{
    private readonly IApplicationDbContext _context;

    public ModifyParticipantCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(ModifyParticipantCommand request, CancellationToken cancellationToken)
    {
        Participant? entity = await _context.Participants
            .FindAsync([request.Id], cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        ParticipantType participantType = entity.Type;

        entity.FirstName = participantType == ParticipantType.Individual ? request.FirstName : null;
        entity.LastName = participantType == ParticipantType.Individual ? request.LastName : null;
        entity.Name = participantType == ParticipantType.Company ? request.Name : null;
        entity.IdCode = request.IdCode;

        await _context.SaveChangesAsync(cancellationToken);
    }
}
