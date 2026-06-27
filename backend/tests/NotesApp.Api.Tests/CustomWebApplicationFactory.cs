using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using NotesApp.Infrastructure.Persistence;

namespace NotesApp.Api.Tests;

public sealed class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly SqliteConnection _connection;

    public CustomWebApplicationFactory()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<NotesAppDbContext>));
            services.RemoveAll(typeof(NotesAppDbContext));
            services.RemoveAll(typeof(IDbContextOptionsConfiguration<NotesAppDbContext>));
            services.RemoveAll(typeof(IConfigureOptions<DbContextOptions<NotesAppDbContext>>));

            services.AddDbContext<NotesAppDbContext>(options => options.UseSqlite(_connection));

            using var scope = services.BuildServiceProvider().CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<NotesAppDbContext>();
            db.Database.EnsureCreated();
        });
    }

    public override async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();
        await _connection.DisposeAsync();
    }
}
