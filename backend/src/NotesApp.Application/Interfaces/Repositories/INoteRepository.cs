using NotesApp.Domain.Entities;

namespace NotesApp.Application.Interfaces;

public interface INoteRepository
{
    /// <summary>
    /// Retrieves a note by its unique identifier.
    /// </summary>
    Task<Note?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all notes owned by the specified user.
    /// </summary>
    Task<IReadOnlyList<Note>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Persists a new note.
    /// </summary>
    Task AddAsync(Note note, CancellationToken cancellationToken = default);

    /// <summary>
    /// Persists changes to an existing note.
    /// </summary>
    Task UpdateAsync(Note note, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an existing note.
    /// </summary>
    Task DeleteAsync(Note note, CancellationToken cancellationToken = default);
}
