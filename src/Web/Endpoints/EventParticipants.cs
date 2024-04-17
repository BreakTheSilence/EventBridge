using EventBridge.Application.EventParticipants.Commands.CreateEventExisingParticipant;
using EventBridge.Application.EventParticipants.Commands.CreateEventNewParticipant;

namespace EventBridge.Web.Endpoints;

public class EventParticipants : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .RequireAuthorization()
            .MapPost(CreateEventParticipantFromNewParticipant, "new-participant")
            .MapPost(CreateEventParticipantFromExistingParticipant, "existing-participant");
    }

    public Task<int> CreateEventParticipantFromNewParticipant(ISender sender, CreateEventNewParticipantCommand command)
    {
        return sender.Send(command);
    }
    public Task<int> CreateEventParticipantFromExistingParticipant(ISender sender, CreateEventExisingParticipantCommand command)
    {
        return sender.Send(command);
    }
}
