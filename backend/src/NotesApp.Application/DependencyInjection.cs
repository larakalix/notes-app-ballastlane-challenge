using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using NotesApp.Application.Interfaces;
using NotesApp.Application.Services;

namespace NotesApp.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<INoteService, NoteService>();

        return services;
    }
}
