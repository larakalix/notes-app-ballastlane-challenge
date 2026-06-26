using FluentValidation;
using NotesApp.Domain.Entities;

namespace NotesApp.Application.Validators.Entities;

public sealed class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
        RuleFor(user => user.Id)
            .NotEmpty();

        RuleFor(user => user.Name)
            .NotEmpty()
            .Must(name => !string.IsNullOrWhiteSpace(name));

        RuleFor(user => user.Email)
            .NotEmpty()
            .Must(email => !string.IsNullOrWhiteSpace(email))
            .EmailAddress();

        RuleFor(user => user.PasswordHash)
            .NotEmpty()
            .Must(passwordHash => !string.IsNullOrWhiteSpace(passwordHash));
    }
}
