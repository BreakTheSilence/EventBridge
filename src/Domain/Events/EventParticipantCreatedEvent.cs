namespace EventBridge.Domain.Events;

public class EventParticipantCreatedEvent : BaseEvent
{
    public EventParticipantCreatedEvent(EventParticipant eventParticipant)
    {
        EventParticipant = eventParticipant;
    }

    public EventParticipant EventParticipant { get; set; }
}
