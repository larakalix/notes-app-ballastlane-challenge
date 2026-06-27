using NotesApp.Application.Interfaces;
using NotesApp.Domain.Entities;

namespace NotesApp.Infrastructure.Persistence;

public static class DemoDataSeeder
{
    public static async Task SeedAsync(NotesAppDbContext dbContext, IPasswordHasher passwordHasher, CancellationToken cancellationToken = default)
    {
        if (dbContext.Users.Any())
        {
            return;
        }

        var now = DateTime.UtcNow;
        var passwordHash = passwordHasher.Hash("demo00123");

        var userOne = new User
        {
            Name = "Demo User One",
            Email = "demo1@notesapp.local",
            PasswordHash = passwordHash,
            CreatedAt = now
        };

        var userTwo = new User
        {
            Name = "Demo User Two",
            Email = "demo2@notesapp.local",
            PasswordHash = passwordHash,
            CreatedAt = now
        };

        dbContext.Users.AddRange(userOne, userTwo);

        var notes = new List<Note>
        {
            new()
            {
                Title = "Welcome note",
                Content = "This is a seeded note for demo user one.",
                Status = "active",
                UserId = userOne.Id,
                DueDate = now.AddDays(2),
                CreatedAt = now,
                UpdatedAt = now
            },
            new()
            {
                Title = "Shopping list",
                Content = "Milk, bread, and coffee.",
                Status = "inactive",
                UserId = userOne.Id,
                DueDate = now.AddDays(4),
                CreatedAt = now,
                UpdatedAt = now
            },
            new()
            {
                Title = "Meeting prep",
                Content = "Prepare agenda and review notes.",
                Status = "active",
                UserId = userOne.Id,
                DueDate = now.AddDays(1),
                CreatedAt = now,
                UpdatedAt = now
            },
            new()
            {
                Title = "Project ideas",
                Content = "Collect MVP ideas for the next sprint.",
                Status = "active",
                UserId = userTwo.Id,
                DueDate = now.AddDays(3),
                CreatedAt = now,
                UpdatedAt = now
            },
            new()
            {
                Title = "Travel checklist",
                Content = "Passport, chargers, and itinerary.",
                Status = "inactive",
                UserId = userTwo.Id,
                DueDate = now.AddDays(6),
                CreatedAt = now,
                UpdatedAt = now
            },
            new()
            {
                Title = "Daily journal",
                Content = "Write a short summary of today.",
                Status = "active",
                UserId = userTwo.Id,
                DueDate = now.AddDays(5),
                CreatedAt = now,
                UpdatedAt = now
            }
        };

        dbContext.Notes.AddRange(notes);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
