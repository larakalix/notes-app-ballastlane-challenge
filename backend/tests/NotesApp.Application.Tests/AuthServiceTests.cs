using FluentAssertions;
using FluentValidation;
using Moq;
using NotesApp.Application.DTOs.Auth;
using NotesApp.Application.Interfaces;
using NotesApp.Application.Services;
using NotesApp.Application.Validators.Entities;
using NotesApp.Application.Validators.Requests;
using NotesApp.Domain.Entities;

namespace NotesApp.Application.Tests;

public class AuthServiceTests
{
    private readonly Mock<IUserRepository> _userRepository = new();
    private readonly Mock<IPasswordHasher> _passwordHasher = new();
    private readonly Mock<ITokenService> _tokenService = new();
    private readonly IValidator<RegisterRequest> _registerValidator = new RegisterRequestValidator();
    private readonly IValidator<LoginRequest> _loginValidator = new LoginRequestValidator();
    private readonly IValidator<User> _userValidator = new UserValidator();

    private AuthService CreateService()
        => new(
            _userRepository.Object,
            _passwordHasher.Object,
            _tokenService.Object,
            _registerValidator,
            _loginValidator,
            _userValidator);

    [Fact]
    public async Task RegisterAsync_Succeeds_WithValidData()
    {
        var request = new RegisterRequest("Ivan", "ivan@example.com", "Pass123!");
        _userRepository.Setup(x => x.EmailExistsAsync("ivan@example.com", It.IsAny<CancellationToken>())).ReturnsAsync(false);
        _passwordHasher.Setup(x => x.Hash(request.Password)).Returns("hashed");
        _tokenService.Setup(x => x.GenerateToken(It.IsAny<User>())).Returns("jwt-token");

        var service = CreateService();

        var response = await service.RegisterAsync(request);

        response.Token.Should().Be("jwt-token");
        response.User.Email.Should().Be("ivan@example.com");
        _userRepository.Verify(x => x.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Theory]
    [InlineData("", "ivan@example.com", "Pass123!")]
    [InlineData("Ivan", "", "Pass123!")]
    [InlineData("Ivan", "ivan@example.com", "")]
    public async Task RegisterAsync_Fails_WhenAnyRequiredFieldIsEmpty(string name, string email, string password)
    {
        var service = CreateService();
        var request = new RegisterRequest(name, email, password);

        var act = () => service.RegisterAsync(request);

        await act.Should().ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task RegisterAsync_Fails_WhenEmailAlreadyExists()
    {
        _userRepository.Setup(x => x.EmailExistsAsync("ivan@example.com", It.IsAny<CancellationToken>())).ReturnsAsync(true);
        var service = CreateService();

        var act = () => service.RegisterAsync(new RegisterRequest("Ivan", "ivan@example.com", "Pass123!"));

        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task RegisterAsync_HashesPassword_BeforeSaving()
    {
        var request = new RegisterRequest("Ivan", "ivan@example.com", "Pass123!");
        _userRepository.Setup(x => x.EmailExistsAsync("ivan@example.com", It.IsAny<CancellationToken>())).ReturnsAsync(false);
        _passwordHasher.Setup(x => x.Hash(request.Password)).Returns("hashed-password");
        _tokenService.Setup(x => x.GenerateToken(It.IsAny<User>())).Returns("token");
        User? captured = null;
        _userRepository.Setup(x => x.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
            .Callback<User, CancellationToken>((u, _) => captured = u)
            .Returns(Task.CompletedTask);

        var service = CreateService();
        await service.RegisterAsync(request);

        _passwordHasher.Verify(x => x.Hash(request.Password), Times.Once);
        captured.Should().NotBeNull();
        captured!.PasswordHash.Should().Be("hashed-password");
    }

    [Fact]
    public async Task LoginAsync_Succeeds_WithValidCredentials()
    {
        var user = new User { Id = Guid.NewGuid(), Name = "Ivan", Email = "ivan@example.com", PasswordHash = "stored-hash" };
        _userRepository.Setup(x => x.GetByEmailAsync("ivan@example.com", It.IsAny<CancellationToken>())).ReturnsAsync(user);
        _passwordHasher.Setup(x => x.Verify("Pass123!", "stored-hash")).Returns(true);
        _tokenService.Setup(x => x.GenerateToken(user)).Returns("jwt-token");
        var service = CreateService();

        var response = await service.LoginAsync(new LoginRequest("ivan@example.com", "Pass123!"));

        response.Token.Should().Be("jwt-token");
        response.User.Id.Should().Be(user.Id);
    }

    [Fact]
    public async Task LoginAsync_Fails_WhenUserDoesNotExist()
    {
        _userRepository.Setup(x => x.GetByEmailAsync("ivan@example.com", It.IsAny<CancellationToken>())).ReturnsAsync((User?)null);
        var service = CreateService();

        var act = () => service.LoginAsync(new LoginRequest("ivan@example.com", "Pass123!"));

        await act.Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Fact]
    public async Task LoginAsync_Fails_WhenPasswordIsInvalid()
    {
        var user = new User { Id = Guid.NewGuid(), Name = "Ivan", Email = "ivan@example.com", PasswordHash = "stored-hash" };
        _userRepository.Setup(x => x.GetByEmailAsync("ivan@example.com", It.IsAny<CancellationToken>())).ReturnsAsync(user);
        _passwordHasher.Setup(x => x.Verify("Pass123!", "stored-hash")).Returns(false);
        var service = CreateService();

        var act = () => service.LoginAsync(new LoginRequest("ivan@example.com", "Pass123!"));

        await act.Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Fact]
    public async Task GetMeAsync_Succeeds_WhenUserExists()
    {
        var user = new User { Id = Guid.NewGuid(), Name = "Ivan", Email = "ivan@example.com", CreatedAt = DateTime.UtcNow };
        _userRepository.Setup(x => x.GetByIdAsync(user.Id, It.IsAny<CancellationToken>())).ReturnsAsync(user);
        var service = CreateService();

        var response = await service.GetMeAsync(user.Id);

        response.Id.Should().Be(user.Id);
        response.Email.Should().Be(user.Email);
    }

    [Fact]
    public async Task GetMeAsync_Fails_WhenUserDoesNotExist()
    {
        var userId = Guid.NewGuid();
        _userRepository.Setup(x => x.GetByIdAsync(userId, It.IsAny<CancellationToken>())).ReturnsAsync((User?)null);
        var service = CreateService();

        var act = () => service.GetMeAsync(userId);

        await act.Should().ThrowAsync<KeyNotFoundException>();
    }
}
