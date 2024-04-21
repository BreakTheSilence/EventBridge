using EventBridge.Application.Events.Queries.GetEventWithParticipants;
using EventBridge.Domain.Entities;

namespace EventBridge.Application.FunctionalTests.Events.Queries;

public class GetEventWithParticipantsTests : BaseTestFixture
{
    [Test]
    public async Task ShouldReturnEventWithParticipants()
    {
        await Testing.RunAsDefaultUserAsync();

        var newEvent = new Event { Name = "Event", Date = DateTime.Now, Location = "Location", Description = "Description" };
        await Testing.AddAsync(newEvent);

        var participant1 = new Participant { FirstName = "John", LastName = "Doe" };
        var participant2 = new Participant { FirstName = "Jane", LastName = "Doe" };
        await Testing.AddAsync(participant1);
        await Testing.AddAsync(participant2);

        var eventParticipant1 = new EventParticipant { EventId = newEvent.Id, ParticipantId = participant1.Id };
        var eventParticipant2 = new EventParticipant { EventId = newEvent.Id, ParticipantId = participant2.Id };
        await Testing.AddAsync(eventParticipant1);
        await Testing.AddAsync(eventParticipant2);

        var query = new GetEventWithParticipantsQuery(newEvent.Id);

        var result = await Testing.SendAsync(query);

        result.Should().NotBeNull();
        result.Id.Should().Be(newEvent.Id);
        result.EventParticipants.Should().HaveCount(2);
    }

    [Test]
    public async Task ShouldReturnNotFoundForInvalidEventId()
    {
        await Testing.RunAsDefaultUserAsync();
        
        var query = new GetEventWithParticipantsQuery(-1);

        Func<Task> act = async () => await Testing.SendAsync(query);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task ShouldHandleEmptyParticipantsListGracefully()
    {
        await Testing.RunAsDefaultUserAsync();

        var newEvent = new Event { Name = "Solo Event", Date = DateTime.Now, Location = "Solo Location", Description = "Solo Description" };
        await Testing.AddAsync(newEvent);

        var query = new GetEventWithParticipantsQuery(newEvent.Id);

        var result = await Testing.SendAsync(query);

        result.EventParticipants.Should().BeEmpty();
    }

    [Test]
    public async Task ShouldOnlyReturnParticipantsForSpecifiedEvent()
    {
        await Testing.RunAsDefaultUserAsync();

        var event1 = new Event { Name = "Event 1", Date = DateTime.Now.AddDays(-1), Location = "Location 1", Description = "Description 1" };
        var event2 = new Event { Name = "Event 2", Date = DateTime.Now.AddDays(-2), Location = "Location 2", Description = "Description 2" };
        await Testing.AddAsync(event1);
        await Testing.AddAsync(event2);

        var participant1 = new Participant { FirstName = "John", LastName = "Doe" };
        var participant2 = new Participant { FirstName = "Jane", LastName = "Doe" };
        await Testing.AddAsync(participant1);
        await Testing.AddAsync(participant2);

        var eventParticipant1 = new EventParticipant { EventId = event1.Id, ParticipantId = participant1.Id };
        var eventParticipant2 = new EventParticipant { EventId = event2.Id, ParticipantId = participant2.Id };
        await Testing.AddAsync(eventParticipant1);
        await Testing.AddAsync(eventParticipant2);

        var query = new GetEventWithParticipantsQuery(event1.Id);

        var result = await Testing.SendAsync(query);

        result.EventParticipants.Should().ContainSingle().Which.Id.Should().Be(participant1.Id);
    }

    [Test]
    public async Task ShouldApplyMappingsCorrectly()
    {
        await Testing.RunAsDefaultUserAsync();

        var newEvent = new Event { Name = "Mapped Event", Date = DateTime.Now, Location = "Mapped Location", Description = "Mapped Description" };
        await Testing.AddAsync(newEvent);

        var participant = new Participant { FirstName = "Mapped", LastName = "Participant" };
        await Testing.AddAsync(participant);

        var eventParticipant = new EventParticipant { EventId = newEvent.Id, ParticipantId = participant.Id };
        await Testing.AddAsync(eventParticipant);

        var query = new GetEventWithParticipantsQuery(newEvent.Id);

        var result = await Testing.SendAsync(query);
        
        result.EventParticipants.First().FirstName.Should().Be(participant.FirstName);
        result.EventParticipants.First().LastName.Should().Be(participant.LastName);
    }
}
