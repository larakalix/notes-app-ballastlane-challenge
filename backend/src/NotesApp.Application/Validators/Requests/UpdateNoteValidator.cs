using FluentValidation;
using NotesApp.Application.DTOs.Notes;

namespace NotesApp.Application.Validators.Requests;

public sealed class UpdateNoteValidator : AbstractValidator<UpdateNote>
{
    public UpdateNoteValidator()
    {
        RuleFor(request => request.Title)
            .NotEmpty()
            .Must(title => !string.IsNullOrWhiteSpace(title));

        RuleFor(request => request.Content)
            .NotEmpty()
            .Must(content => !string.IsNullOrWhiteSpace(content));
    }
}
