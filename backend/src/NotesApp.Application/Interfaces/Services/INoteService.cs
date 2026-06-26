using NotesApp.Application.DTOs.Notes;

namespace NotesApp.Application.Interfaces;

public interface INoteService
{
    Task<NoteResponse> CreateAsync(Guid userId, CreateNote request, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<NoteResponse>> GetByUserAsync(Guid userId, CancellationToken cancellationToken = default);

    Task<NoteResponse> GetByIdAsync(Guid userId, Guid noteId, CancellationToken cancellationToken = default);

    Task<NoteResponse> UpdateAsync(Guid userId, Guid noteId, UpdateNote request, CancellationToken cancellationToken = default);

    Task DeleteAsync(Guid userId, Guid noteId, CancellationToken cancellationToken = default);
}
