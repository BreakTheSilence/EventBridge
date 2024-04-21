using EventBridge.Application.Participants.Queries.GetParticipant;
using EventBridge.Domain.Entities;
using EventBridge.Domain.Enums;

namespace EventBridge.Application.FunctionalTests.Participants.Queries;

public class GetParticipantTests : BaseTestFixture
{
    [Test]
    public async Task ShouldReturnParticipantById()
    {
        await Testing.RunAsDefaultUserAsync();

        var participant = new Participant { FirstName = "John", LastName = "Doe", Type = ParticipantType.Individual, IdCode = 12345 };
        await Testing.AddAsync(participant);

        var query = new GetParticipantQuery(participant.Id);

        var result = await Testing.SendAsync(query);

        result.Id.Should().Be(participant.Id);
        result.FirstName.Should().Be("John");
        result.LastName.Should().Be("Doe");
    }

    [Test]
    public async Task ShouldThrowNotFoundWhenParticipantDoesNotExist()
    {
        await Testing.RunAsDefaultUserAsync();

        var query = new GetParticipantQuery(999);

        Func<Task> act = async () => await Testing.SendAsync(query);

        await act.Should().ThrowAsync<KeyNotFoundException>();
    }

    [Test]
    public async Task ShouldMapFieldsCorrectly()
    {
        await Testing.RunAsDefaultUserAsync();

        var participant = new Participant { FirstName = "Jane", LastName = "Smith", Type = ParticipantType.Company, IdCode = 67890 };
        await Testing.AddAsync(participant);

        var query = new GetParticipantQuery(participant.Id);

        var result = await Testing.SendAsync(query);

        result.FirstName.Should().Be("Jane");
        result.LastName.Should().Be("Smith");
        result.Type.Should().Be((int)ParticipantType.Company);
        result.IdCode.Should().Be(67890);
    }

    [Test]
    public async Task ShouldReturnNullForEmptyOptionalFields()
    {
        await Testing.RunAsDefaultUserAsync();

        var participant = new Participant { Type = ParticipantType.Individual, IdCode = 12345 };
        await Testing.AddAsync(participant);

        var query = new GetParticipantQuery(participant.Id);

        var result = await Testing.SendAsync(query);

        result.FirstName.Should().BeNull();
        result.LastName.Should().BeNull();
    }

    [Test]
    public async Task ShouldHandleMultipleParticipants()
    {
        await Testing.RunAsDefaultUserAsync();

        var participant1 = new Participant { FirstName = "John", LastName = "Doe", Type = ParticipantType.Individual, IdCode = 12345 };
        var participant2 = new Participant { FirstName = "Jane", LastName = "Doe", Type = ParticipantType.Individual, IdCode = 67890 };
        await Testing.AddAsync(participant1);
        await Testing.AddAsync(participant2);

        var query1 = new GetParticipantQuery(participant1.Id);
        var query2 = new GetParticipantQuery(participant2.Id);

        var result1 = await Testing.SendAsync(query1);
        var result2 = await Testing.SendAsync(query2);

        result1.Id.Should().Be(participant1.Id);
        result2.Id.Should().Be(participant2.Id);
    }
}
