namespace EventBridge.Domain.Events;

public class ParticipantCreatedEvent : BaseEvent
{
    public ParticipantCreatedEvent(Participant participant)
    {
        Participant = participant;
    }

    public Participant Participant { get; set; }
}
