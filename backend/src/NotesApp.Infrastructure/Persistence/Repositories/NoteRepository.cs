using Microsoft.EntityFrameworkCore;
using NotesApp.Application.Interfaces;
using NotesApp.Domain.Entities;

namespace NotesApp.Infrastructure.Persistence.Repositories;

public sealed class NoteRepository(NotesAppDbContext dbContext) : INoteRepository
{
    private readonly NotesAppDbContext _dbContext = dbContext;

    public Task<Note?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => _dbContext.Notes.FirstOrDefaultAsync(note => note.Id == id, cancellationToken);

    public async Task<IReadOnlyList<Note>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        => await _dbContext.Notes
            .Where(note => note.UserId == userId)
            .OrderByDescending(note => note.UpdatedAt)
            .ToListAsync(cancellationToken);

    public async Task AddAsync(Note note, CancellationToken cancellationToken = default)
    {
        _dbContext.Notes.Add(note);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Note note, CancellationToken cancellationToken = default)
    {
        _dbContext.Notes.Update(note);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Note note, CancellationToken cancellationToken = default)
    {
        _dbContext.Notes.Remove(note);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
