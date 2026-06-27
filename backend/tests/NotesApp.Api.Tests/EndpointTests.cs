using System.Net;
using System.Net.Http.Json;
using FluentAssertions;

namespace NotesApp.Api.Tests;

public class EndpointTests(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task PublicStatus_Returns200Ok()
    {
        var response = await _client.GetAsync("/api/public/status");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Register_ReturnsSuccess_ForValidData()
    {
        var email = $"user-{Guid.NewGuid():N}@example.com";
        var payload = new
        {
            name = "Ivan",
            email,
            password = "Pass123!"
        };

        var response = await _client.PostAsJsonAsync("/api/auth/register", payload);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Login_ReturnsSuccess_ForValidCredentials()
    {
        var email = $"user-{Guid.NewGuid():N}@example.com";
        var registerPayload = new
        {
            name = "Ivan",
            email,
            password = "Pass123!"
        };
        await _client.PostAsJsonAsync("/api/auth/register", registerPayload);

        var loginPayload = new
        {
            email,
            password = "Pass123!"
        };

        var response = await _client.PostAsJsonAsync("/api/auth/login", loginPayload);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetMe_Returns401Unauthorized_WithoutToken()
    {
        var response = await _client.GetAsync("/api/auth/me");

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GetNotes_Returns401Unauthorized_WithoutToken()
    {
        var response = await _client.GetAsync("/api/notes");

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task CreateNote_Returns401Unauthorized_WithoutToken()
    {
        var payload = new
        {
            title = "My note",
            content = "Protected endpoint",
            status = "active",
            dueDate = (DateTime?)null
        };

        var response = await _client.PostAsJsonAsync("/api/notes", payload);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
