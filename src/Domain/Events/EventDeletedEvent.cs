namespace EventBridge.Domain.Events;

public class EventDeletedEvent : BaseEvent
{
    public EventDeletedEvent(Event @event)
    {
        Event = @event;
    }
    public Event Event { get; set; }

}
