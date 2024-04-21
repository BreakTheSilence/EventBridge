using EventBridge.Domain.Constants;
using EventBridge.Domain.Entities;
using EventBridge.Domain.Enums;
using EventBridge.Infrastructure.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EventBridge.Infrastructure.Data;

public static class InitialiserExtensions
{
    public static async Task InitialiseDatabaseAsync(this WebApplication app)
    {
        using IServiceScope scope = app.Services.CreateScope();

        ApplicationDbContextInitialiser initialiser =
            scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();

        await initialiser.InitialiseAsync();

        await initialiser.SeedAsync();
    }
}

public class ApplicationDbContextInitialiser
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ApplicationDbContextInitialiser> _logger;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public ApplicationDbContextInitialiser(ILogger<ApplicationDbContextInitialiser> logger,
        ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            await _context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {
        // Default roles
        IdentityRole administratorRole = new IdentityRole(Roles.Administrator);

        if (_roleManager.Roles.All(r => r.Name != administratorRole.Name))
        {
            await _roleManager.CreateAsync(administratorRole);
        }

        // Default users
        ApplicationUser administrator =
            new ApplicationUser { UserName = "administrator@localhost", Email = "administrator@localhost" };

        if (_userManager.Users.All(u => u.UserName != administrator.UserName))
        {
            await _userManager.CreateAsync(administrator, "Administrator1!");
            if (!string.IsNullOrWhiteSpace(administratorRole.Name))
            {
                await _userManager.AddToRolesAsync(administrator, new[] { administratorRole.Name });
            }
        }

        // Default data
        // Seed, if necessary
        if (!_context.Events.Any())
        {
            Event firstEvent = new Event
            {
                Name = "Conference",
                Date = new DateTime(2024, 5, 1),
                Location = "Virtual",
                Description = "A conference on technology and innovation."
            };

            Event secondEvent = new Event
            {
                Name = "Workshop",
                Date = new DateTime(2024, 5, 15),
                Location = "In-person",
                Description = "A hands-on workshop for developers."
            };

            _context.Events.AddRange(firstEvent, secondEvent);
            await _context.SaveChangesAsync();

            Participant participant1 = new Participant
            {
                Type = ParticipantType.Individual, FirstName = "John", LastName = "Doe", IdCode = 255968703867
            };

            Participant participant2 = new Participant
            {
                Type = ParticipantType.Individual, FirstName = "Alice", LastName = "Smith", IdCode = 254444703867
            };
            Participant participant3 = new Participant
            {
                Type = ParticipantType.Company, Name = "ÕÄ OÜ", IdCode = 254443867
            };

            _context.Participants.AddRange(participant1, participant2, participant3);
            await _context.SaveChangesAsync();

            _context.EventParticipants.AddRange(
                new EventParticipant
                {
                    Event = firstEvent,
                    Participant = participant1,
                    PaymentMethod = PaymentMethod.BankTransfer,
                    ParticipantsCount = 1,
                    AdditionalInfo = "Registered early bird"
                },
                new EventParticipant
                {
                    Event = firstEvent,
                    Participant = participant2,
                    PaymentMethod = PaymentMethod.Cash,
                    ParticipantsCount = 1,
                    AdditionalInfo = "Speaker"
                }
            );
            await _context.SaveChangesAsync();
        }

        await _context.SaveChangesAsync();
    }
}
