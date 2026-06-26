namespace NotesApp.Application.DTOs.Notes;

public sealed record CreateNote(
    string Title,
    string Content,
    string Status,
    DateTime? DueDate
);
