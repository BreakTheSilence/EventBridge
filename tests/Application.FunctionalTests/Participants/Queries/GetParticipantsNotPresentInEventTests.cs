using EventBridge.Application.Participants.Queries.GetParticipantsNotPresentInEvent;
using EventBridge.Domain.Entities;

namespace EventBridge.Application.FunctionalTests.Participants.Queries;

public class GetParticipantsNotPresentInEventTests : BaseTestFixture
{
    [Test]
    public async Task ShouldReturnParticipantsNotRegisteredForEvent()
    {
        await Testing.RunAsDefaultUserAsync();

        var event1 = new Event { Name = "Event 1" };
        var event2 = new Event { Name = "Event 2" };
        await Testing.AddAsync(event1);
        await Testing.AddAsync(event2);

        var participant1 = new Participant { FirstName = "John" };
        var participant2 = new Participant { FirstName = "Jane" };
        await Testing.AddAsync(participant1);
        await Testing.AddAsync(participant2);

        var eventParticipant = new EventParticipant { EventId = event1.Id, ParticipantId = participant1.Id };
        await Testing.AddAsync(eventParticipant);

        var query = new GetParticipantsNotPresentInEventQuery(event1.Id);

        var result = await Testing.SendAsync(query);

        result.Should().ContainSingle();
        result.First().Id.Should().Be(participant2.Id);
    }

    [Test]
    public async Task ShouldReturnAllParticipantsIfNoneRegistered()
    {
        await Testing.RunAsDefaultUserAsync();

        var event1 = new Event { Name = "Event 1" };
        await Testing.AddAsync(event1);

        var participant1 = new Participant { FirstName = "John" };
        var participant2 = new Participant { FirstName = "Jane" };
        await Testing.AddAsync(participant1);
        await Testing.AddAsync(participant2);

        var query = new GetParticipantsNotPresentInEventQuery(event1.Id);

        var result = await Testing.SendAsync(query);

        result.Should().HaveCount(2);
    }

    [Test]
    public async Task ShouldReturnEmptyListIfAllRegistered()
    {
        await Testing.RunAsDefaultUserAsync();

        var event1 = new Event { Name = "Event 1" };
        await Testing.AddAsync(event1);

        var participant1 = new Participant { FirstName = "John" };
        var participant2 = new Participant { FirstName = "Jane" };
        await Testing.AddAsync(participant1);
        await Testing.AddAsync(participant2);

        await Testing.AddAsync(new EventParticipant { EventId = event1.Id, ParticipantId = participant1.Id });
        await Testing.AddAsync(new EventParticipant { EventId = event1.Id, ParticipantId = participant2.Id });

        var query = new GetParticipantsNotPresentInEventQuery(event1.Id);

        var result = await Testing.SendAsync(query);

        result.Should().BeEmpty();
    }

    [Test]
    public async Task ShouldNotReturnParticipantsFromOtherEvents()
    {
        await Testing.RunAsDefaultUserAsync();

        var event1 = new Event { Name = "Event 1" };
        var event2 = new Event { Name = "Event 2" };
        await Testing.AddAsync(event1);
        await Testing.AddAsync(event2);

        var participant1 = new Participant { FirstName = "John" };
        var participant2 = new Participant { FirstName = "Jane" };
        await Testing.AddAsync(participant1);
        await Testing.AddAsync(participant2);

        await Testing.AddAsync(new EventParticipant { EventId = event2.Id, ParticipantId = participant1.Id });

        var query = new GetParticipantsNotPresentInEventQuery(event1.Id);

        var result = await Testing.SendAsync(query);

        result.Should().ContainSingle(p => p.Id == participant2.Id);
    }

    [Test]
    public async Task ShouldHandleInvalidEventIdGracefully()
    {
        await Testing.RunAsDefaultUserAsync();

        var query = new GetParticipantsNotPresentInEventQuery(999);

        var result = await Testing.SendAsync(query);

        result.Should().BeEmpty();
    }
}
