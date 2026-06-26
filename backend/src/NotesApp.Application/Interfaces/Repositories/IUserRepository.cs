using NotesApp.Domain.Entities;

namespace NotesApp.Application.Interfaces;

public interface IUserRepository
{
    /// <summary>
    /// Retrieves a user by its unique identifier.
    /// </summary>
    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a user by email.
    /// </summary>
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks whether a user with the given email already exists.
    /// </summary>
    Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default);

    /// <summary>
    /// Persists a new user.
    /// </summary>
    Task AddAsync(User user, CancellationToken cancellationToken = default);
}
