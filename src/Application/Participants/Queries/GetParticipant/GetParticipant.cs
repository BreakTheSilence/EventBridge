using EventBridge.Application.Common.Interfaces;

namespace EventBridge.Application.Participants.Queries.GetParticipant;

public record GetParticipantQuery(int Id) : IRequest<ParticipantDto>
{
}

public class GetParticipantQueryValidator : AbstractValidator<GetParticipantQuery>
{
}

public class GetParticipantQueryHandler : IRequestHandler<GetParticipantQuery, ParticipantDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetParticipantQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ParticipantDto> Handle(GetParticipantQuery request, CancellationToken cancellationToken)
    {
        ParticipantDto? participantDto = await _context.Participants
            .Where(p => p.Id == request.Id)
            .ProjectTo<ParticipantDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        if (participantDto == null)
        {
            throw new KeyNotFoundException($"Participant with Id {request.Id} not found.");
        }

        return participantDto;
    }
}
