namespace EventBridge.Domain.Events;

public class EventCreatedEvent : BaseEvent
{
    public EventCreatedEvent(Event @event)
    {
        Event = @event;
    }

    public Event Event { get; set; }
}
