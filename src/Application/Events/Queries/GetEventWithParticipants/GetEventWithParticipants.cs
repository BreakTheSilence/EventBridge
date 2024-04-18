using EventBridge.Application.Common.Interfaces;

namespace EventBridge.Application.Events.Queries.GetEventWithParticipants;

public record GetEventWithParticipantsQuery(int Id) : IRequest<EventWithParticipantsDto>
{
}

public class GetEventWithParticipantsQueryValidator : AbstractValidator<GetEventWithParticipantsQuery>
{
    public GetEventWithParticipantsQueryValidator()
    {
    }
}

public class GetEventWithParticipantsQueryHandler : IRequestHandler<GetEventWithParticipantsQuery, EventWithParticipantsDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetEventWithParticipantsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<EventWithParticipantsDto> Handle(GetEventWithParticipantsQuery request, CancellationToken cancellationToken)
    {
        var eventWithParticipants = await _context.Events
            .Include(e => e.EventParticipants)
            .ThenInclude(ep => ep.Participant)
            .ProjectTo<EventWithParticipantsDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

          Guard.Against.NotFound(request.Id, eventWithParticipants);

        return eventWithParticipants;
    }
}
