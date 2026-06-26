using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NotesApp.Domain.Entities;

namespace NotesApp.Infrastructure.Persistence;

public class NotesAppDbContext(DbContextOptions<NotesAppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();

    public DbSet<Note> Notes => Set<Note>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(NotesAppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(user => user.Id);

        builder.Property(user => user.Id)
            .HasColumnType("uuid")
            .IsRequired();

        builder.Property(user => user.Name)
            .HasColumnType("text")
            .IsRequired();

        builder.Property(user => user.Email)
            .HasColumnType("text")
            .IsRequired();

        builder.Property(user => user.PasswordHash)
            .HasColumnType("text")
            .IsRequired();

        builder.Property(user => user.CreatedAt)
            .HasColumnType("timestamp with time zone")
            .HasDefaultValueSql("now()")
            .IsRequired();

        builder.HasIndex(user => user.Email)
            .IsUnique();
    }
}

public class NoteConfiguration : IEntityTypeConfiguration<Note>
{
    public void Configure(EntityTypeBuilder<Note> builder)
    {
        builder.ToTable("notes");

        builder.HasKey(note => note.Id);

        builder.Property(note => note.Id)
            .HasColumnType("uuid")
            .IsRequired();

        builder.Property(note => note.Title)
            .HasColumnType("text")
            .IsRequired();

        builder.Property(note => note.Content)
            .HasColumnType("text")
            .IsRequired();

        builder.Property(note => note.Status)
            .HasColumnName("status")
            .HasColumnType("text")
            .HasDefaultValue("active")
            .IsRequired();

        builder.Property(note => note.DueDate)
            .HasColumnName("due_date")
            .HasColumnType("timestamp with time zone")
            .IsRequired(false);

        builder.Property(note => note.UserId)
            .HasColumnType("uuid")
            .IsRequired();

        builder.Property(note => note.CreatedAt)
            .HasColumnType("timestamp with time zone")
            .HasDefaultValueSql("now()")
            .IsRequired();

        builder.Property(note => note.UpdatedAt)
            .HasColumnType("timestamp with time zone")
            .HasDefaultValueSql("now()")
            .IsRequired();

        builder.HasOne(note => note.User)
            .WithMany(user => user.Notes)
            .HasForeignKey(note => note.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
