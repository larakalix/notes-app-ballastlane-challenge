using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NotesApp.Domain.Entities;
using NotesApp.Infrastructure.Persistence;
using NotesApp.Infrastructure.Persistence.Repositories;

namespace NotesApp.Infrastructure.Tests;

public class NoteRepositoryTests
{
    private static NotesAppDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<NotesAppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new NotesAppDbContext(options);
    }

    [Fact]
    public async Task AddAsync_PersistsNote()
    {
        await using var db = CreateDbContext();
        var repo = new NoteRepository(db);
        var note = new Note { Id = Guid.NewGuid(), Title = "Title", Content = "Content", Status = "active", UserId = Guid.NewGuid() };

        await repo.AddAsync(note);

        (await db.Notes.CountAsync()).Should().Be(1);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNote_WhenExists()
    {
        await using var db = CreateDbContext();
        var note = new Note { Id = Guid.NewGuid(), Title = "Title", Content = "Content", Status = "active", UserId = Guid.NewGuid() };
        db.Notes.Add(note);
        await db.SaveChangesAsync();
        var repo = new NoteRepository(db);

        var result = await repo.GetByIdAsync(note.Id);

        result.Should().NotBeNull();
        result!.Title.Should().Be("Title");
    }

    [Fact]
    public async Task GetByUserIdAsync_ReturnsOnlyNotesForSpecifiedUser()
    {
        await using var db = CreateDbContext();
        var userId = Guid.NewGuid();
        db.Notes.AddRange(
            new Note { Id = Guid.NewGuid(), Title = "A", Content = "A", Status = "active", UserId = userId, UpdatedAt = DateTime.UtcNow.AddMinutes(-1) },
            new Note { Id = Guid.NewGuid(), Title = "B", Content = "B", Status = "inactive", UserId = userId, UpdatedAt = DateTime.UtcNow },
            new Note { Id = Guid.NewGuid(), Title = "C", Content = "C", Status = "active", UserId = Guid.NewGuid() });
        await db.SaveChangesAsync();
        var repo = new NoteRepository(db);

        var notes = await repo.GetByUserIdAsync(userId);

        notes.Should().HaveCount(2);
        notes.All(x => x.UserId == userId).Should().BeTrue();
        notes[0].UpdatedAt.Should().BeAfter(notes[1].UpdatedAt);
    }

    [Fact]
    public async Task UpdateAsync_PersistsChanges()
    {
        await using var db = CreateDbContext();
        var note = new Note { Id = Guid.NewGuid(), Title = "Old", Content = "Old", Status = "active", UserId = Guid.NewGuid() };
        db.Notes.Add(note);
        await db.SaveChangesAsync();
        var repo = new NoteRepository(db);

        note.Title = "New";
        note.Status = "inactive";
        await repo.UpdateAsync(note);

        var saved = await db.Notes.FirstAsync(x => x.Id == note.Id);
        saved.Title.Should().Be("New");
        saved.Status.Should().Be("inactive");
    }

    [Fact]
    public async Task DeleteAsync_RemovesNote()
    {
        await using var db = CreateDbContext();
        var note = new Note { Id = Guid.NewGuid(), Title = "Title", Content = "Content", Status = "active", UserId = Guid.NewGuid() };
        db.Notes.Add(note);
        await db.SaveChangesAsync();
        var repo = new NoteRepository(db);

        await repo.DeleteAsync(note);

        (await db.Notes.CountAsync()).Should().Be(0);
    }
}
