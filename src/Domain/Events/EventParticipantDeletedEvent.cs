namespace EventBridge.Domain.Events;

public class EventParticipantDeletedEvent : BaseEvent
{
    public EventParticipantDeletedEvent(EventParticipant eventParticipant)
    {
        EventParticipant = eventParticipant;
    }

    public EventParticipant EventParticipant { get; set; }
}
