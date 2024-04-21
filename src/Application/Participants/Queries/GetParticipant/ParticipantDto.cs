using EventBridge.Domain.Entities;

namespace EventBridge.Application.Participants.Queries.GetParticipant;

public class ParticipantDto
{
    public int Id { get; set; }
    public int Type { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Name { get; set; }
    public long IdCode { get; set; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Participant, ParticipantDto>();
        }
    }
}
