using FluentValidation;
using NotesApp.Application.DTOs.Auth;

namespace NotesApp.Application.Validators.Requests;

public sealed class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(request => request.Email)
            .NotEmpty()
            .Must(email => !string.IsNullOrWhiteSpace(email))
            .EmailAddress();

        RuleFor(request => request.Password)
            .NotEmpty()
            .Must(password => !string.IsNullOrWhiteSpace(password));
    }
}
