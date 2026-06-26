using FluentValidation;
using NotesApp.Application.DTOs.Notes;

namespace NotesApp.Application.Validators.Requests;

public sealed class CreateNoteValidator : AbstractValidator<CreateNote>
{
    public CreateNoteValidator()
    {
        RuleFor(request => request.Title)
            .NotEmpty()
            .Must(title => !string.IsNullOrWhiteSpace(title));

        RuleFor(request => request.Content)
            .NotEmpty()
            .Must(content => !string.IsNullOrWhiteSpace(content));
    }
}
