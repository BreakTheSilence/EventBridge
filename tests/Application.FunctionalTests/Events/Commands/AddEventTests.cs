using EventBridge.Application.Common.Exceptions;
using EventBridge.Application.Events.Commands.AddEvent;
using EventBridge.Domain.Entities;

namespace EventBridge.Application.FunctionalTests.Events.Commands;

public class AddEventTests : BaseTestFixture
{
    [Test]
    public async Task ShouldSuccessfullyAddEvent()
    {
        await Testing.RunAsDefaultUserAsync();

        var command = new AddEventCommand
        {
            Name = "Test Event",
            Date = DateTime.UtcNow,
            Location = "Test Location",
            Description = "Test Description"
        };

        var eventId = await Testing.SendAsync(command);

        var dbEvent = await Testing.FindAsync<Event>(eventId);

        dbEvent.Should().NotBeNull();
        dbEvent?.Name.Should().Be(command.Name);
        dbEvent?.Date.Should().BeCloseTo(command.Date, TimeSpan.FromSeconds(1));
        dbEvent?.Location.Should().Be(command.Location);
        dbEvent?.Description.Should().Be(command.Description);
    }
    

    [Test]
    public async Task ShouldFailToAddEventWithPastDate()
    {
        await Testing.RunAsDefaultUserAsync();

        var command = new AddEventCommand
        {
            Name = "Past Event",
            Date = DateTime.UtcNow.AddDays(-1),
            Location = "Past Location",
            Description = "Past Description"
        };

        Func<Task> act = async () => await Testing.SendAsync(command);

        await act.Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldIgnoreEmptyLocationAndDescription()
    {
        await Testing.RunAsDefaultUserAsync();

        var command = new AddEventCommand
        {
            Name = "Event without Location",
            Date = DateTime.UtcNow
        };

        var eventId = await Testing.SendAsync(command);

        var dbEvent = await Testing.FindAsync<Event>(eventId);

        dbEvent?.Location.Should().BeEmpty();
        dbEvent?.Description.Should().BeEmpty();
    }

    [Test]
    public async Task ShouldNotAllowDuplicateEventNamesOnTheSameDate()
    {
        await Testing.RunAsDefaultUserAsync();

        var command1 = new AddEventCommand
        {
            Name = "Duplicate Event",
            Date = DateTime.UtcNow,
            Location = "Duplicate Location 1",
            Description = "Duplicate Description 1"
        };

        var eventId1 = await Testing.SendAsync(command1);

        var command2 = new AddEventCommand
        {
            Name = "Duplicate Event",
            Date = command1.Date, // Same date as the first event
            Location = "Duplicate Location 2",
            Description = "Duplicate Description 2"
        };

        Func<Task> act = async () => await Testing.SendAsync(command2);

        await act.Should().ThrowAsync<ValidationException>();
    }
}
