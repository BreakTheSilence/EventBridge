using EventBridge.Application.Common.Interfaces;
using EventBridge.Application.Common.Mappings;
using EventBridge.Application.Common.Models;

namespace EventBridge.Application.Events.Queries.GetEvents;

public record GetEventsQuery : IRequest<PaginatedList<EventDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetEventsQueryValidator : AbstractValidator<GetEventsQuery>
{
    public GetEventsQueryValidator()
    {
    }
}

public class GetEventsQueryHandler : IRequestHandler<GetEventsQuery, PaginatedList<EventDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetEventsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<EventDto>> Handle(GetEventsQuery request, CancellationToken cancellationToken)
    {
        return await _context.Events
            .OrderByDescending(x => x.Date)
            .ProjectTo<EventDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
