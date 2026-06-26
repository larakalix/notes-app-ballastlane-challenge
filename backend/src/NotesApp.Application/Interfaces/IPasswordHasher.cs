namespace NotesApp.Application.Interfaces;

public interface IPasswordHasher
{
    /// <summary>
    /// Creates a secure hash for a plain text password.
    /// </summary>
    string Hash(string password);

    /// <summary>
    /// Verifies whether a plain text password matches a stored hash.
    /// </summary>
    bool Verify(string password, string passwordHash);
}
