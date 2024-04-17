namespace EventBridge.Domain.Entities;

public class EventParticipant : BaseAuditableEntity
{
    public int EventId { get; set; }
    public Event? Event { get; set; }
    public int ParticipantId { get; set; }
    public Participant? Participant { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public int ParticipantsCount { get; set; }
    public string? AdditionalInfo { get; set; }
}
