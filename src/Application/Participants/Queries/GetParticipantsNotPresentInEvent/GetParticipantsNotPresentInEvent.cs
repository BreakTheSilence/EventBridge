using EventBridge.Application.Common.Interfaces;
using EventBridge.Application.Participants.Queries.GetParticipant;

namespace EventBridge.Application.Participants.Queries.GetParticipantsNotPresentInEvent;

public record GetParticipantsNotPresentInEventQuery(int EventId) : IRequest<List<ParticipantDto>>
{
}

public class GetParticipantsNotPresentInEventQueryValidator : AbstractValidator<GetParticipantsNotPresentInEventQuery>
{
}

public class
    GetParticipantsNotPresentInEventQueryHandler : IRequestHandler<GetParticipantsNotPresentInEventQuery,
    List<ParticipantDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetParticipantsNotPresentInEventQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ParticipantDto>> Handle(GetParticipantsNotPresentInEventQuery request,
        CancellationToken cancellationToken)
    {
        IQueryable<int> registeredParticipantIds = _context.EventParticipants
            .Where(ep => ep.EventId == request.EventId)
            .Select(ep => ep.ParticipantId)
            .Distinct();

        IQueryable<ParticipantDto>? unregisteredParticipants = _context.Participants
            .Where(p => !registeredParticipantIds.Contains(p.Id))
            .ProjectTo<ParticipantDto>(_mapper.ConfigurationProvider);

        return await unregisteredParticipants.ToListAsync(cancellationToken);
    }
}
