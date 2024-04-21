using EventBridge.Application.Events.Queries.GetEvents;
using EventBridge.Domain.Entities;

namespace EventBridge.Application.FunctionalTests.Events.Queries;

public class GetEventsTests : BaseTestFixture
{
    [Test]
    public async Task ShouldReturnEventsWithPagination()
    {
        await Testing.RunAsDefaultUserAsync();

        var newEvent1 = new Event { Name = "Event 1", Date = DateTime.Now.AddDays(-1), Location = "Location 1", Description = "Description 1" };
        var newEvent2 = new Event { Name = "Event 2", Date = DateTime.Now.AddDays(-2), Location = "Location 2", Description = "Description 2" };
        var newEvent3 = new Event { Name = "Event 3", Date = DateTime.Now, Location = "Location 3", Description = "Description 3" };

        await Testing.AddAsync(newEvent1);
        await Testing.AddAsync(newEvent2);
        await Testing.AddAsync(newEvent3);

        var query = new GetEventsQuery { PageNumber = 1, PageSize = 2 };

        var result = await Testing.SendAsync(query);

        result.Items.Should().HaveCount(2);
        result.Items.First().Name.Should().Be("Event 3");
        result.TotalCount.Should().Be(3);
        result.TotalPages.Should().Be(2);
    }
    
    [Test]
    public async Task ShouldReturnEmptyListWhenNoEventsExist()
    {
        await Testing.RunAsDefaultUserAsync();

        var query = new GetEventsQuery { PageNumber = 1, PageSize = 10 };

        var result = await Testing.SendAsync(query);

        result.TotalCount.Should().Be(0);
        result.Items.Should().BeEmpty();
    }
    
    [Test]
    public async Task ShouldLimitEventsToPageSize()
    {
        await Testing.RunAsDefaultUserAsync();
        
        for (int i = 1; i <= 15; i++)
        {
            await Testing.AddAsync(new Event { Name = $"Event {i}", Date = DateTime.Now.AddDays(-i) });
        }

        var query = new GetEventsQuery { PageNumber = 1, PageSize = 5 };

        var result = await Testing.SendAsync(query);

        result.Items.Should().HaveCount(5);
        result.TotalPages.Should().Be(3);
    }
    
    [Test]
    public async Task ShouldSortEventsByDateDescending()
    {
        await Testing.RunAsDefaultUserAsync();

        var dates = new[] { DateTime.Now, DateTime.Now.AddDays(-1), DateTime.Now.AddDays(1) };
        foreach (var date in dates)
        {
            await Testing.AddAsync(new Event { Name = $"Event {date.ToShortDateString()}", Date = date });
        }

        var query = new GetEventsQuery { PageNumber = 1, PageSize = 10 };

        var result = await Testing.SendAsync(query);

        var expectedOrder = dates.OrderByDescending(d => d).ToList();
        var actualOrder = result.Items.Select(e => e.Date).ToList();
        actualOrder.Should().Equal(expectedOrder);
    }
    
    [Test]
    public async Task ShouldHandleNonExistentPageGracefully()
    {
        await Testing.RunAsDefaultUserAsync();

        var query = new GetEventsQuery { PageNumber = 100, PageSize = 10 };

        var result = await Testing.SendAsync(query);

        result.Items.Should().BeEmpty();
        result.TotalCount.Should().Be(0);
    }
}
