using EventBridge.Application.EventParticipants.Commands.CreateEventExisingParticipant;
using EventBridge.Domain.Entities;
using EventBridge.Domain.Enums;

namespace EventBridge.Application.FunctionalTests.EventParticipants.Commands;

public class CreateEventExisingParticipantTests : BaseTestFixture
{
    [Test]
    public async Task ShouldReturnExistingEventParticipantId()
    {
        await Testing.RunAsDefaultUserAsync();

        var newEvent = new Event { Name = "Event 1" };
        var participant = new Participant { Type = ParticipantType.Individual };
        await Testing.AddAsync(newEvent);
        await Testing.AddAsync(participant);

        var eventParticipant = new EventParticipant
        {
            EventId = newEvent.Id,
            ParticipantId = participant.Id,
            PaymentMethod = PaymentMethod.Cash
        };
        await Testing.AddAsync(eventParticipant);

        var command = new CreateEventExisingParticipantCommand
        {
            EventId = newEvent.Id,
            ParticipantId = participant.Id,
            PaymentMethod = (int)PaymentMethod.Cash,
            ParticipationCount = 1
        };

        var result = await Testing.SendAsync(command);

        result.Should().Be(eventParticipant.Id);
    }

    [Test]
    public async Task ShouldRejectCreationIfEventOrParticipantNotExist()
    {
        await Testing.RunAsDefaultUserAsync();

        var command = new CreateEventExisingParticipantCommand
        {
            EventId = 999,
            ParticipantId = 999,
            PaymentMethod = (int)PaymentMethod.Cash,
            ParticipationCount = 1
        };

        var result = await Testing.SendAsync(command);

        result.Should().Be(-1);
    }
}

