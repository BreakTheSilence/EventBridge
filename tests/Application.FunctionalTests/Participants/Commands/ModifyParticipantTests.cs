using EventBridge.Application.Participants.Commands.ModifyParticipant;
using EventBridge.Domain.Entities;
using EventBridge.Domain.Enums;

namespace EventBridge.Application.FunctionalTests.Participants.Commands;

public class ModifyParticipantTests : BaseTestFixture
{
    [Test]
    public async Task ShouldUpdateParticipantDetails()
    {
        await Testing.RunAsDefaultUserAsync();

        var participant = new Participant { Type = ParticipantType.Individual, FirstName = "Old Name", LastName = "Old Last", IdCode = 12345 };
        await Testing.AddAsync(participant);

        var command = new ModifyParticipantCommand { Id = participant.Id, FirstName = "New Name", LastName = "New Last", IdCode = 67890 };
        await Testing.SendAsync(command);

        var updatedParticipant = await Testing.FindAsync<Participant>(participant.Id);

        updatedParticipant!.FirstName.Should().Be("New Name");
        updatedParticipant.LastName.Should().Be("New Last");
        updatedParticipant.IdCode.Should().Be(67890);
    }

    [Test]
    public async Task ShouldNotUpdateNameForIndividual()
    {
        await Testing.RunAsDefaultUserAsync();

        var participant = new Participant { Type = ParticipantType.Individual, Name = "Company Name", IdCode = 12345 };
        await Testing.AddAsync(participant);

        var command = new ModifyParticipantCommand { Id = participant.Id, Name = "New Company Name", IdCode = 67890 };
        await Testing.SendAsync(command);

        var updatedParticipant = await Testing.FindAsync<Participant>(participant.Id);

        updatedParticipant!.Name.Should().BeNull();
    }

    [Test]
    public async Task ShouldUpdateNameForCompany()
    {
        await Testing.RunAsDefaultUserAsync();

        var participant = new Participant { Type = ParticipantType.Company, Name = "Old Company Name", IdCode = 12345 };
        await Testing.AddAsync(participant);

        var command = new ModifyParticipantCommand { Id = participant.Id, Name = "New Company Name", IdCode = 67890 };
        await Testing.SendAsync(command);

        var updatedParticipant = await Testing.FindAsync<Participant>(participant.Id);

        updatedParticipant!.Name.Should().Be("New Company Name");
    }

    [Test]
    public async Task ShouldThrowNotFoundForInvalidId()
    {
        await Testing.RunAsDefaultUserAsync();

        var command = new ModifyParticipantCommand { Id = 999, FirstName = "John", LastName = "Doe", IdCode = 67890 };

        Func<Task> act = async () => await Testing.SendAsync(command);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task ShouldPreserveUnchangedFields()
    {
        await Testing.RunAsDefaultUserAsync();

        var participant = new Participant { Type = ParticipantType.Individual, FirstName = "Initial", LastName = "Person", IdCode = 12345 };
        await Testing.AddAsync(participant);

        var command = new ModifyParticipantCommand { Id = participant.Id, FirstName = "Updated" };
        await Testing.SendAsync(command);

        var updatedParticipant = await Testing.FindAsync<Participant>(participant.Id);

        updatedParticipant!.FirstName.Should().Be("Updated");
        updatedParticipant.LastName.Should().Be("Person");
        updatedParticipant.IdCode.Should().Be(12345);
    }
}
