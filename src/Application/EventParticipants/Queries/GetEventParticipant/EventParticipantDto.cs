using EventBridge.Application.Participants.Queries.GetParticipant;
using EventBridge.Domain.Entities;

namespace EventBridge.Application.EventParticipants.Queries.GetEventParticipant;

public class EventParticipantDto
{
    public int Id { get; set; }
    public int EventId { get; set; }
    public int PaymentMethod { get; set; }
    public required ParticipantDto Participant { get; set; }
    public int ParticipantsCount { get; set; }
    public string? AdditionalInfo { get; set; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<EventParticipant, EventParticipantDto>();
            CreateMap<Participant, ParticipantDto>();
        }
    }
}
