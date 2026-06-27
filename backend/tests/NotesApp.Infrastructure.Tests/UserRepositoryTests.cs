using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NotesApp.Domain.Entities;
using NotesApp.Infrastructure.Persistence;
using NotesApp.Infrastructure.Persistence.Repositories;

namespace NotesApp.Infrastructure.Tests;

public class UserRepositoryTests
{
    private static NotesAppDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<NotesAppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new NotesAppDbContext(options);
    }

    [Fact]
    public async Task AddAsync_PersistsUser()
    {
        await using var db = CreateDbContext();
        var repo = new UserRepository(db);
        var user = new User { Id = Guid.NewGuid(), Name = "Ivan", Email = "ivan@example.com", PasswordHash = "hash" };

        await repo.AddAsync(user);

        (await db.Users.CountAsync()).Should().Be(1);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsUser_WhenExists()
    {
        await using var db = CreateDbContext();
        var user = new User { Id = Guid.NewGuid(), Name = "Ivan", Email = "ivan@example.com", PasswordHash = "hash" };
        db.Users.Add(user);
        await db.SaveChangesAsync();
        var repo = new UserRepository(db);

        var result = await repo.GetByIdAsync(user.Id);

        result.Should().NotBeNull();
        result!.Email.Should().Be(user.Email);
    }

    [Fact]
    public async Task GetByEmailAsync_ReturnsUser_WhenExists()
    {
        await using var db = CreateDbContext();
        var user = new User { Id = Guid.NewGuid(), Name = "Ivan", Email = "ivan@example.com", PasswordHash = "hash" };
        db.Users.Add(user);
        await db.SaveChangesAsync();
        var repo = new UserRepository(db);

        var result = await repo.GetByEmailAsync("ivan@example.com");

        result.Should().NotBeNull();
        result!.Id.Should().Be(user.Id);
    }

    [Fact]
    public async Task EmailExistsAsync_ReturnsTrue_WhenEmailExists()
    {
        await using var db = CreateDbContext();
        db.Users.Add(new User { Id = Guid.NewGuid(), Name = "Ivan", Email = "ivan@example.com", PasswordHash = "hash" });
        await db.SaveChangesAsync();
        var repo = new UserRepository(db);

        var exists = await repo.EmailExistsAsync("ivan@example.com");

        exists.Should().BeTrue();
    }

    [Fact]
    public async Task EmailExistsAsync_ReturnsFalse_WhenEmailDoesNotExist()
    {
        await using var db = CreateDbContext();
        var repo = new UserRepository(db);

        var exists = await repo.EmailExistsAsync("nobody@example.com");

        exists.Should().BeFalse();
    }
}
