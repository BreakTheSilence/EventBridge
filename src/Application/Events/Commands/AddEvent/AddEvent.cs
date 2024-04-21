using EventBridge.Application.Common.Interfaces;
using EventBridge.Domain.Entities;
using EventBridge.Domain.Events;

namespace EventBridge.Application.Events.Commands.AddEvent;

public record AddEventCommand : IRequest<int>
{
    public required string Name { get; set; }
    public DateTime Date { get; set; }
    public string Location { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

public class AddEventCommandValidator : AbstractValidator<AddEventCommand>
{
}

public class AddEventCommandHandler : IRequestHandler<AddEventCommand, int>
{
    private readonly IApplicationDbContext _context;

    public AddEventCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(AddEventCommand request, CancellationToken cancellationToken)
    {
        Event entity = new Event
        {
            Name = request.Name, Date = request.Date, Location = request.Location, Description = request.Description
        };

        entity.AddDomainEvent(new EventCreatedEvent(entity));

        _context.Events.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
