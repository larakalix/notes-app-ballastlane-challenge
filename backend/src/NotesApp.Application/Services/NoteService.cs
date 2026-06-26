using FluentValidation;
using NotesApp.Application.Common.Exceptions;
using NotesApp.Application.DTOs.Notes;
using NotesApp.Application.Interfaces;
using NotesApp.Domain.Entities;

namespace NotesApp.Application.Services;

public sealed class NoteService(
    INoteRepository noteRepository,
    IValidator<CreateNote> createNoteValidator,
    IValidator<UpdateNote> updateNoteValidator,
    IValidator<Note> noteValidator) : INoteService
{
    private readonly INoteRepository _noteRepository = noteRepository;
    private readonly IValidator<CreateNote> _createNoteValidator = createNoteValidator;
    private readonly IValidator<UpdateNote> _updateNoteValidator = updateNoteValidator;
    private readonly IValidator<Note> _noteValidator = noteValidator;

    public async Task<NoteResponse> CreateAsync(Guid userId, CreateNote request, CancellationToken cancellationToken = default)
    {
        if (userId == Guid.Empty)
        {
            throw new ValidationException("User id is required.");
        }

        await _createNoteValidator.ValidateAndThrowAsync(request, cancellationToken);

        var now = DateTime.UtcNow;
        var note = new Note
        {
            Title = request.Title.Trim(),
            Content = request.Content.Trim(),
            Status = request.Status.Trim().ToLowerInvariant(),
            DueDate = request.DueDate,
            UserId = userId,
            CreatedAt = now,
            UpdatedAt = now
        };

        await _noteValidator.ValidateAndThrowAsync(note, cancellationToken);
        await _noteRepository.AddAsync(note, cancellationToken);

        return ToResponse(note);
    }

    public async Task<IReadOnlyList<NoteResponse>> GetByUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        if (userId == Guid.Empty)
        {
            throw new ValidationException("User id is required.");
        }

        var notes = await _noteRepository.GetByUserIdAsync(userId, cancellationToken);
        return notes.Select(ToResponse).ToList();
    }

    public async Task<NoteResponse> GetByIdAsync(Guid userId, Guid noteId, CancellationToken cancellationToken = default)
    {
        var note = await GetOwnedNoteOrThrowAsync(userId, noteId, cancellationToken);
        return ToResponse(note);
    }

    public async Task<NoteResponse> UpdateAsync(Guid userId, Guid noteId, UpdateNote request, CancellationToken cancellationToken = default)
    {
        await _updateNoteValidator.ValidateAndThrowAsync(request, cancellationToken);

        var note = await GetOwnedNoteOrThrowAsync(userId, noteId, cancellationToken);
        note.Title = request.Title.Trim();
        note.Content = request.Content.Trim();
        note.Status = request.Status.Trim().ToLowerInvariant();
        note.DueDate = request.DueDate;
        note.UpdatedAt = DateTime.UtcNow;

        await _noteValidator.ValidateAndThrowAsync(note, cancellationToken);
        await _noteRepository.UpdateAsync(note, cancellationToken);

        return ToResponse(note);
    }

    public async Task DeleteAsync(Guid userId, Guid noteId, CancellationToken cancellationToken = default)
    {
        var note = await GetOwnedNoteOrThrowAsync(userId, noteId, cancellationToken);
        await _noteRepository.DeleteAsync(note, cancellationToken);
    }

    private async Task<Note> GetOwnedNoteOrThrowAsync(Guid userId, Guid noteId, CancellationToken cancellationToken)
    {
        if (userId == Guid.Empty)
        {
            throw new ValidationException("User id is required.");
        }

        if (noteId == Guid.Empty)
        {
            throw new ValidationException("Note id is required.");
        }

        var note = await _noteRepository.GetByIdAsync(noteId, cancellationToken)
            ?? throw new KeyNotFoundException("Note not found.");

        if (note.UserId != userId)
        {
            throw new ForbiddenAccessException("You do not have access to this note.");
        }

        return note;
    }

    private static NoteResponse ToResponse(Note note)
        => new(note.Id, note.Title, note.Content, note.Status, note.DueDate, note.UserId, note.CreatedAt, note.UpdatedAt);
}
