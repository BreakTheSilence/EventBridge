using EventBridge.Domain.Entities;

namespace EventBridge.Application.Events.Queries.GetEvents;

public class EventDto
{
    public int Id { get; init; }
    public required string Name { get; init; }
    public DateTime Date { get; init; }
    public string Location { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Event, EventDto>();
        }
    }
}
