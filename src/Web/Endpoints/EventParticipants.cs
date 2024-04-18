using EventBridge.Application.EventParticipants.Commands.CreateEventExisingParticipant;
using EventBridge.Application.EventParticipants.Commands.CreateEventNewParticipant;
using EventBridge.Application.EventParticipants.Commands.DeleteEventParticipant;

namespace EventBridge.Web.Endpoints;

public class EventParticipants : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .RequireAuthorization()
            .MapPost(CreateEventParticipantFromNewParticipant, "new-participant")
            .MapPost(CreateEventParticipantFromExistingParticipant, "existing-participant")
            .MapDelete(DeleteEventParticipant, "{eventId:int}/{participantId:int}");
    }

    public Task<int> CreateEventParticipantFromNewParticipant(ISender sender, CreateEventNewParticipantCommand command)
    {
        return sender.Send(command);
    }
    public Task<int> CreateEventParticipantFromExistingParticipant(ISender sender, CreateEventExisingParticipantCommand command)
    {
        return sender.Send(command);
    }

    public async Task<IResult> DeleteEventParticipant(ISender sender, int eventId, int participantId)
    {
        await sender.Send(new DeleteEventParticipantCommand(eventId, participantId));
        return Results.NoContent();
    }
}
