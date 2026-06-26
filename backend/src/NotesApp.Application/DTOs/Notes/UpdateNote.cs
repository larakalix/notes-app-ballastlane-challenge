namespace NotesApp.Application.DTOs.Notes;

public sealed record UpdateNote(
    string Title,
    string Content,
    string Status,
    DateTime? DueDate
);
