using EventBridge.Application.Common.Models;
using EventBridge.Application.Events.Commands.AddEvent;
using EventBridge.Application.Events.Queries.GetEvents;

namespace EventBridge.Web.Endpoints;

public class Events : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .RequireAuthorization()
            .MapPost(CreateEvent)
            .MapGet(GetEventsWithPagination);
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
}
