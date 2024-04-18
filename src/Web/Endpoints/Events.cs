using EventBridge.Application.Common.Models;
using EventBridge.Application.Events.Commands.AddEvent;
using EventBridge.Application.Events.Queries.GetEvents;
using EventBridge.Application.Events.Queries.GetEventWithParticipants;

namespace EventBridge.Web.Endpoints;

public class Events : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .RequireAuthorization()
            .MapPost(CreateEvent)
            .MapGet(GetEventsWithPagination)
            .MapGet(GetEventWithParticipants, "{id}");
    }

    public Task<int> CreateEvent(ISender sender, AddEventCommand command)
    {
        return sender.Send(command);
    }

    public Task<PaginatedList<EventDto>> GetEventsWithPagination(ISender sender,
        [AsParameters] GetEventsQuery query)
    {
        return sender.Send(query);
    }

    public Task<EventWithParticipantsDto> GetEventWithParticipants(ISender sender, int id)
    {
        return sender.Send(new GetEventWithParticipantsQuery(id));
    }
}
