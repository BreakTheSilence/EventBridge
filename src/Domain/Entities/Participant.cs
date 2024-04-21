namespace EventBridge.Domain.Entities;

public class Participant : BaseAuditableEntity
{
    public ParticipantType Type { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Name { get; set; }
    public long IdCode { get; set; }

    public virtual ICollection<EventParticipant> EventParticipants { get; set; } = new List<EventParticipant>();
}
