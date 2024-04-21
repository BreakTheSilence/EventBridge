using EventBridge.Application.EventParticipants.Queries.GetEventParticipant;
using EventBridge.Domain.Entities;
using EventBridge.Domain.Enums;

namespace EventBridge.Application.FunctionalTests.EventParticipants.Queries;

public class GetEventParticipantTests : BaseTestFixture
{
    [Test]
    public async Task ShouldReturnEventParticipantDetails()
    {
        await Testing.RunAsDefaultUserAsync();

        var newEvent = new Event { Name = "Event" };
        var participant = new Participant { FirstName = "John" };
        await Testing.AddAsync(newEvent);
        await Testing.AddAsync(participant);

        var eventParticipant = new EventParticipant
        {
            EventId = newEvent.Id,
            ParticipantId = participant.Id,
            PaymentMethod = PaymentMethod.Cash,
            ParticipantsCount = 1,
            AdditionalInfo = "Info"
        };
        await Testing.AddAsync(eventParticipant);

        var query = new GetEventParticipantQuery(newEvent.Id, participant.Id);

        var result = await Testing.SendAsync(query);

        result.EventId.Should().Be(newEvent.Id);
        result.Participant.Id.Should().Be(participant.Id);
        result.PaymentMethod.Should().Be((int)PaymentMethod.Cash);
        result.ParticipantsCount.Should().Be(1);
        result.AdditionalInfo.Should().Be("Info");
    }

    [Test]
    public async Task ShouldThrowNotFoundWhenParticipantOrEventDoesNotExist()
    {
        await Testing.RunAsDefaultUserAsync();

        var query = new GetEventParticipantQuery(999, 999);

        Func<Task> act = async () => await Testing.SendAsync(query);

        await act.Should().ThrowAsync<KeyNotFoundException>();
    }

    [Test]
    public async Task ShouldReturnCorrectParticipantForEvent()
    {
        await Testing.RunAsDefaultUserAsync();

        var event1 = new Event { Name = "Event 1" };
        var event2 = new Event { Name = "Event 2" };
        var participant = new Participant { FirstName = "John" };
        await Testing.AddAsync(event1);
        await Testing.AddAsync(event2);
        await Testing.AddAsync(participant);

        await Testing.AddAsync(new EventParticipant { EventId = event1.Id, ParticipantId = participant.Id });
        await Testing.AddAsync(new EventParticipant { EventId = event2.Id, ParticipantId = participant.Id });

        var query = new GetEventParticipantQuery(event1.Id, participant.Id);

        var result = await Testing.SendAsync(query);

        result.EventId.Should().Be(event1.Id);
    }

    [Test]
    public async Task ShouldNotReturnParticipantNotRegisteredToEvent()
    {
        await Testing.RunAsDefaultUserAsync();

        var event1 = new Event { Name = "Event 1" };
        var participant = new Participant { FirstName = "John" };
        await Testing.AddAsync(event1);
        await Testing.AddAsync(participant);

        var query = new GetEventParticipantQuery(event1.Id, participant.Id);

        Func<Task> act = async () => await Testing.SendAsync(query);

        await act.Should().ThrowAsync<KeyNotFoundException>();
    }

    [Test]
    public async Task ShouldHandleMultipleParticipantsInEvent()
    {
        await Testing.RunAsDefaultUserAsync();

        var event1 = new Event { Name = "Event 1" };
        var participant1 = new Participant { FirstName = "John" };
        var participant2 = new Participant { FirstName = "Jane" };
        await Testing.AddAsync(event1);
        await Testing.AddAsync(participant1);
        await Testing.AddAsync(participant2);

        await Testing.AddAsync(new EventParticipant { EventId = event1.Id, ParticipantId = participant1.Id });
        await Testing.AddAsync(new EventParticipant { EventId = event1.Id, ParticipantId = participant2.Id });

        var query1 = new GetEventParticipantQuery(event1.Id, participant1.Id);
        var query2 = new GetEventParticipantQuery(event1.Id, participant2.Id);

        var result1 = await Testing.SendAsync(query1);
        var result2 = await Testing.SendAsync(query2);

        result1.Participant.Id.Should().Be(participant1.Id);
        result2.Participant.Id.Should().Be(participant2.Id);
    }
}
