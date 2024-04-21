namespace EventBridge.Domain.Entities;

public class Event : BaseAuditableEntity
{
    public required string Name { get; init; }
    public DateTime Date { get; set; }
    public string Location { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public virtual ICollection<EventParticipant> EventParticipants { get; set; } = new List<EventParticipant>();
}
