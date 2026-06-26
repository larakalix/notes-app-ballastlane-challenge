using NotesApp.Application.DTOs.Users;

namespace NotesApp.Application.DTOs.Auth;

public sealed record AuthResponse(
    string Token,
    UserResponse User
);
