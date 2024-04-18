using EventBridge.Domain.Entities;

namespace EventBridge.Application.Events.Queries.GetEventWithParticipants;

public class EventWithParticipantsDto
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string? Location { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;

    public IEnumerable<ParticipantDto> EventParticipants { get; set; } = Array.Empty<ParticipantDto>();
    
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Event, EventWithParticipantsDto>()
                .ForMember(dest => dest.EventParticipants, opt => opt.MapFrom(src => 
                    src.EventParticipants.Select(ep => ep.Participant)));
            
            CreateMap<Participant, ParticipantDto>();
        }
    }
}
