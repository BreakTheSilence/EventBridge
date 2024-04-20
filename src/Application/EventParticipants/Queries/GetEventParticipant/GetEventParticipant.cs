using EventBridge.Application.Common.Interfaces;

namespace EventBridge.Application.EventParticipants.Queries.GetEventParticipant;

public record GetEventParticipantQuery(int EventId, int ParticipantId) : IRequest<EventParticipantDto>
{
}

public class GetEventParticipantQueryValidator : AbstractValidator<GetEventParticipantQuery>
{
    public GetEventParticipantQueryValidator()
    {
    }
}

public class GetEventParticipantQueryHandler : IRequestHandler<GetEventParticipantQuery, EventParticipantDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetEventParticipantQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<EventParticipantDto> Handle(GetEventParticipantQuery request, CancellationToken cancellationToken)
    {

        var entity = await _context.EventParticipants
            .ProjectTo<EventParticipantDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e =>
                e.Participant.Id.Equals(request.ParticipantId) && e.EventId.Equals(request.EventId), cancellationToken);

        if (entity == null)
        {
            throw new KeyNotFoundException($"Entity with EventId {request.EventId} and ParticipantId {request.ParticipantId} not found.");
        }

        return entity;
    }
}
