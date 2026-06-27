using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NotesApp.Api.Middleware;
using NotesApp.Application;
using NotesApp.Application.Interfaces;
using NotesApp.Infrastructure;
using NotesApp.Infrastructure.Persistence;
using NotesApp.Infrastructure.Security;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
var allowedOrigin = builder.Configuration["Cors:AllowedOrigin"] ?? "http://localhost:3080";

builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendCors", policy =>
    {
        policy
            .WithOrigins(allowedOrigin)
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddAuthorization();

var jwtOptions = builder.Configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>()
    ?? throw new InvalidOperationException("Jwt configuration is required.");

if (string.IsNullOrWhiteSpace(jwtOptions.SecretKey))
{
    throw new InvalidOperationException("Jwt:SecretKey configuration is required.");
}

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtOptions.Issuer,
            ValidAudience = jwtOptions.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey))
        };
    });
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<NotesAppDbContext>();
    await dbContext.Database.EnsureCreatedAsync();

    if (dbContext.Database.ProviderName?.Contains("Npgsql", StringComparison.OrdinalIgnoreCase) == true)
    {
        await dbContext.Database.ExecuteSqlRawAsync(
            "ALTER TABLE IF EXISTS notes ADD COLUMN IF NOT EXISTS status text NOT NULL DEFAULT 'active';");
        await dbContext.Database.ExecuteSqlRawAsync(
            "ALTER TABLE IF EXISTS notes ADD COLUMN IF NOT EXISTS due_date timestamp with time zone NULL;");
    }

    var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();
    await DemoDataSeeder.SeedAsync(dbContext, passwordHasher);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();

app.UseCors("FrontendCors");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program;
