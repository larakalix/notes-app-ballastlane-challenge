using FluentAssertions;
using FluentValidation;
using Moq;
using NotesApp.Application.Common.Exceptions;
using NotesApp.Application.DTOs.Notes;
using NotesApp.Application.Interfaces;
using NotesApp.Application.Services;
using NotesApp.Application.Validators.Entities;
using NotesApp.Application.Validators.Requests;
using NotesApp.Domain.Entities;

namespace NotesApp.Application.Tests;

public class NoteServiceTests
{
    private readonly Mock<INoteRepository> _noteRepository = new();
    private readonly IValidator<CreateNote> _createValidator = new CreateNoteValidator();
    private readonly IValidator<UpdateNote> _updateValidator = new UpdateNoteValidator();
    private readonly IValidator<Note> _noteValidator = new NoteValidator();

    private NoteService CreateService()
        => new(_noteRepository.Object, _createValidator, _updateValidator, _noteValidator);

    [Fact]
    public async Task CreateAsync_Succeeds_WithValidData()
    {
        var userId = Guid.NewGuid();
        var request = new CreateNote("Title", "Content", "active", DateTime.UtcNow.AddDays(2));
        var service = CreateService();

        var response = await service.CreateAsync(userId, request);

        response.Title.Should().Be("Title");
        response.Content.Should().Be("Content");
        response.Status.Should().Be("active");
        response.DueDate.Should().Be(request.DueDate);
        _noteRepository.Verify(x => x.AddAsync(It.IsAny<Note>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Theory]
    [InlineData("", "content")]
    [InlineData("title", "")]
    public async Task CreateAsync_Fails_WhenTitleOrContentIsEmpty(string title, string content)
    {
        var userId = Guid.NewGuid();
        var request = new CreateNote(title, content, "active", null);
        var service = CreateService();

        var act = () => service.CreateAsync(userId, request);

        await act.Should().ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task GetByUserAsync_ReturnsOnlyCurrentUserNotes()
    {
        var userId = Guid.NewGuid();
        _noteRepository.Setup(x => x.GetByUserIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Note>
            {
                new() { Id = Guid.NewGuid(), Title = "A", Content = "C", Status = "active", UserId = userId },
                new() { Id = Guid.NewGuid(), Title = "B", Content = "D", Status = "inactive", UserId = userId }
            });
        var service = CreateService();

        var response = await service.GetByUserAsync(userId);

        response.Should().HaveCount(2);
        response.All(x => x.UserId == userId).Should().BeTrue();
        _noteRepository.Verify(x => x.GetByUserIdAsync(userId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_Succeeds_WhenNoteBelongsToUser()
    {
        var userId = Guid.NewGuid();
        var noteId = Guid.NewGuid();
        _noteRepository.Setup(x => x.GetByIdAsync(noteId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Note { Id = noteId, Title = "A", Content = "B", Status = "active", UserId = userId });
        var service = CreateService();

        var response = await service.GetByIdAsync(userId, noteId);

        response.Id.Should().Be(noteId);
    }

    [Fact]
    public async Task GetByIdAsync_Fails_WhenNoteBelongsToAnotherUser()
    {
        var userId = Guid.NewGuid();
        var noteId = Guid.NewGuid();
        _noteRepository.Setup(x => x.GetByIdAsync(noteId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Note { Id = noteId, Title = "A", Content = "B", Status = "active", UserId = Guid.NewGuid() });
        var service = CreateService();

        var act = () => service.GetByIdAsync(userId, noteId);

        await act.Should().ThrowAsync<ForbiddenAccessException>();
    }

    [Fact]
    public async Task UpdateAsync_Succeeds_WhenNoteBelongsToUser()
    {
        var userId = Guid.NewGuid();
        var noteId = Guid.NewGuid();
        var existing = new Note { Id = noteId, Title = "Old", Content = "Old", Status = "active", UserId = userId };
        _noteRepository.Setup(x => x.GetByIdAsync(noteId, It.IsAny<CancellationToken>())).ReturnsAsync(existing);
        var service = CreateService();
        var request = new UpdateNote("New", "Updated", "inactive", DateTime.UtcNow.AddDays(1));

        var response = await service.UpdateAsync(userId, noteId, request);

        response.Title.Should().Be("New");
        response.Status.Should().Be("inactive");
        response.DueDate.Should().Be(request.DueDate);
        _noteRepository.Verify(x => x.UpdateAsync(existing, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_Fails_WhenNoteBelongsToAnotherUser()
    {
        var userId = Guid.NewGuid();
        var noteId = Guid.NewGuid();
        _noteRepository.Setup(x => x.GetByIdAsync(noteId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Note { Id = noteId, Title = "A", Content = "B", Status = "active", UserId = Guid.NewGuid() });
        var service = CreateService();

        var act = () => service.UpdateAsync(userId, noteId, new UpdateNote("t", "c", "active", null));

        await act.Should().ThrowAsync<ForbiddenAccessException>();
    }

    [Fact]
    public async Task DeleteAsync_Succeeds_WhenNoteBelongsToUser()
    {
        var userId = Guid.NewGuid();
        var noteId = Guid.NewGuid();
        var note = new Note { Id = noteId, Title = "A", Content = "B", Status = "active", UserId = userId };
        _noteRepository.Setup(x => x.GetByIdAsync(noteId, It.IsAny<CancellationToken>())).ReturnsAsync(note);
        var service = CreateService();

        await service.DeleteAsync(userId, noteId);

        _noteRepository.Verify(x => x.DeleteAsync(note, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_Fails_WhenNoteBelongsToAnotherUser()
    {
        var userId = Guid.NewGuid();
        var noteId = Guid.NewGuid();
        _noteRepository.Setup(x => x.GetByIdAsync(noteId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Note { Id = noteId, Title = "A", Content = "B", Status = "active", UserId = Guid.NewGuid() });
        var service = CreateService();

        var act = () => service.DeleteAsync(userId, noteId);

        await act.Should().ThrowAsync<ForbiddenAccessException>();
    }
}
