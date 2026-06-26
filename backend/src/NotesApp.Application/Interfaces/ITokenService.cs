using NotesApp.Domain.Entities;

namespace NotesApp.Application.Interfaces;

public interface ITokenService
{
    /// <summary>
    /// Generates an authentication token for the given user.
    /// </summary>
    string GenerateToken(User user);
}
