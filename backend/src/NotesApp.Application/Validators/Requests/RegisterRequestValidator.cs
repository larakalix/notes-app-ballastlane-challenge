using FluentValidation;
using NotesApp.Application.DTOs.Auth;

namespace NotesApp.Application.Validators.Requests;

public sealed class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(request => request.Name)
            .NotEmpty()
            .Must(name => !string.IsNullOrWhiteSpace(name));

        RuleFor(request => request.Email)
            .NotEmpty()
            .Must(email => !string.IsNullOrWhiteSpace(email))
            .EmailAddress();

        RuleFor(request => request.Password)
            .NotEmpty()
            .Must(password => !string.IsNullOrWhiteSpace(password));
    }
}
