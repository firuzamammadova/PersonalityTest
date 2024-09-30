using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PersonalityTest.Domain.Constants;
using PersonalityTest.Domain.Entities;
using PersonalityTest.Infrastructure.Identity;

namespace PersonalityTest.Infrastructure.Data;
public static class InitialiserExtensions
{
    public static async Task InitialiseDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();

        await initialiser.InitialiseAsync();

        await initialiser.SeedAsync();
    }
}

public class ApplicationDbContextInitialiser
{
    private readonly ILogger<ApplicationDbContextInitialiser> _logger;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public ApplicationDbContextInitialiser(ILogger<ApplicationDbContextInitialiser> logger, ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
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
        var administratorRole = new IdentityRole(Roles.Administrator);

        if (_roleManager.Roles.All(r => r.Name != administratorRole.Name))
        {
            await _roleManager.CreateAsync(administratorRole);
        }

        // Default users
        var administrator = new ApplicationUser { UserName = "administrator@localhost", Email = "administrator@localhost" };

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
        if (!_context.Questions.Any())
        {
            _context.Questions.Add(new Question
            {
                Text = "You’re really busy at work and a colleague is telling you their life story and personal woes. You:",
                Answers = new List<Answer>
            {
                new Answer { AnswerText = "Don’t dare to interrupt them" },
                new Answer { AnswerText = "Think it’s more important to give them some of your time; work can wait" },
                new Answer { AnswerText = "Listen, but with only half an ear" },
                new Answer { AnswerText = "Interrupt and explain that you are really busy at the moment" }
            }
            });

            _context.Questions.Add(new Question
            {
                Text = "You’ve been sitting in the doctor’s waiting room for more than 25 minutes. You:",
                Answers = new List<Answer>
            {
                new Answer { AnswerText = "Look at your watch every two minutes" },
                new Answer { AnswerText = "Bubble with inner anger, but keep quiet" },
                new Answer { AnswerText = "Explain to other equally impatient people in the room that the doctor is always running late" },
                new Answer { AnswerText = "Complain in a loud voice, while tapping your foot impatiently" }
            }
            });

            _context.Questions.Add(new Question
            {
                Text = "You’re having an animated discussion with a colleague regarding a project that you’re in charge of. You:",
                Answers = new List<Answer>
            {
                new Answer { AnswerText = "Don’t dare contradict them" },
                new Answer { AnswerText = "Think that they are obviously right" },
                new Answer { AnswerText = "Defend your own point of view, tooth and nail" },
                new Answer { AnswerText = "Continuously interrupt your colleague" }
            }
            });

            _context.Questions.Add(new Question
            {
                Text = "You are taking part in a guided tour of a museum. You:",
                Answers = new List<Answer>
            {
                new Answer { AnswerText = "Are a bit too far towards the back so don’t really hear what the guide is saying" },
                new Answer { AnswerText = "Follow the group without question" },
                new Answer { AnswerText = "Make sure that everyone is able to hear properly" },
                new Answer { AnswerText = "Are right up the front, adding your own comments in a loud voice" }
            }
            });

            _context.Questions.Add(new Question
            {
                Text = "During dinner parties at your home, you have a hard time with people who:",
                Answers = new List<Answer>
            {
                new Answer { AnswerText = "Ask you to tell a story in front of everyone else" },
                new Answer { AnswerText = "Talk privately between themselves" },
                new Answer { AnswerText = "Hang around you all evening" },
                new Answer { AnswerText = "Always drag the conversation back to themselves" }
            }
            });
            await _context.SaveChangesAsync();

        }

    }
}
