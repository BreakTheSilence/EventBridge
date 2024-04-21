using EventBridge.Application.Participants.Commands.ModifyParticipant;
using EventBridge.Application.Participants.Queries.GetParticipant;
using EventBridge.Application.Participants.Queries.GetParticipantsNotPresentInEvent;

namespace EventBridge.Web.Endpoints;

public class Participants : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapGet(GetParticipantById, "{id}")
            .MapGet(GetParticipantsNotPresentInEventByEventId, "/event/{eventId:int}")
            .MapPut(UpdateParticipant, "{id}");
    }

    public Task<ParticipantDto> GetParticipantById(ISender sender, int id)
    {
        return sender.Send(new GetParticipantQuery(id));
    }

    public Task<List<ParticipantDto>> GetParticipantsNotPresentInEventByEventId(ISender sender, int eventId)
    {
        return sender.Send(new GetParticipantsNotPresentInEventQuery(eventId));
    }
    
    public async Task<IResult> UpdateParticipant(ISender sender, int id, ModifyParticipantCommand command)
    {
        if (id != command.Id) return Results.BadRequest();
        await sender.Send(command);
        return Results.NoContent();
    }
}
