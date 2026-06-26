using FluentValidation;
using NotesApp.Domain.Entities;

namespace NotesApp.Application.Validators.Entities;

public sealed class NoteValidator : AbstractValidator<Note>
{
    public NoteValidator()
    {
        RuleFor(note => note.Id)
            .NotEmpty();

        RuleFor(note => note.Title)
            .NotEmpty()
            .Must(title => !string.IsNullOrWhiteSpace(title));

        RuleFor(note => note.Content)
            .NotEmpty()
            .Must(content => !string.IsNullOrWhiteSpace(content));

        RuleFor(note => note.UserId)
            .NotEmpty();
    }
}
