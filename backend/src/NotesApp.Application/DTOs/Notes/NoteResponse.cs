namespace NotesApp.Application.DTOs.Notes;

public sealed record NoteResponse(
    Guid Id,
    string Title,
    string Content,
    string Status,
    DateTime? DueDate,
    Guid UserId,
    DateTime CreatedAt,
    DateTime UpdatedAt
);
