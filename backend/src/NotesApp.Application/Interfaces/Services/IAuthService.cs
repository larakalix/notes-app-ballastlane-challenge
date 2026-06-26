using NotesApp.Application.DTOs.Auth;
using NotesApp.Application.DTOs.Users;

namespace NotesApp.Application.Interfaces;

public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default);

    Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);

    Task<UserResponse> GetMeAsync(Guid userId, CancellationToken cancellationToken = default);
}
