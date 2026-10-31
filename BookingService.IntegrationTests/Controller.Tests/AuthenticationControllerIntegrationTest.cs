
using System.Net;
using System.Net.Http.Json;
using BookingService.DAL.Interfaces;
using BookingService.IntegrationTests.Infrastructure;
using BookingService.IntegrationTests.Seeder.TestData;
using BookingService.ViewModels.Models;
using Microsoft.Extensions.DependencyInjection;

namespace BookingService.IntegrationTests.Controller.Tests;

[Collection("IntegrationTests")]
public class AuthenticationControllerIntegrationTest
    : IntegrationTestBase
{
    private readonly BookingApplicationWebFactory _factory;

    private const string RegisterUrl = "/api/auth/register";
    private const string LoginUrl = "/api/auth/login";

    private static AuthModel NewUser => new()
    {
        Email = $"test-{Guid.NewGuid():N}@gmail.com",
        Password = "TestPassword123!"
    };

    private static AuthModel ExistingUser => new()
    {
        Email = TestUsers.DefaultUser.Email,
        Password = "TestPassword123!"
    };

    private static AuthModel UnknownUser => new()
    {
        Email = $"unknown-{Guid.NewGuid():N}@gmail.com",
        Password = "TestPassword123!"
    };

    public AuthenticationControllerIntegrationTest(
        BookingApplicationWebFactory factory)
        : base(factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task RegisterUserAsync_ValidUser_ReturnsOk()
    {
        // Arrange
        var model = NewUser;

        // Act
        var response = await Client.PostAsJsonAsync(
            RegisterUrl, model);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        using var scope = _factory.Services.CreateScope();

        var userRepository = scope.ServiceProvider
            .GetRequiredService<IUserRepository>();

        var user = await userRepository.GetByEmailAsync(
            model.Email);

        Assert.NotNull(user);
        Assert.Equal(model.Email, user.Email);
    }

    [Fact]
    public async Task RegisterUserAsync_ExistingEmail_ReturnsConflict()
    {
        // Arrange
        var model = ExistingUser;

        // Act
        var response = await Client.PostAsJsonAsync(
            RegisterUrl, model);

        // Assert
        Assert.Equal(
            HttpStatusCode.Conflict,
            response.StatusCode);
    }

    [Fact]
    public async Task LoginUserAsync_ValidCredentials_ReturnsToken()
    {
        // Arrange
        var model = NewUser;

        var registerResponse = await Client.PostAsJsonAsync(
            RegisterUrl, model);

        Assert.Equal(
            HttpStatusCode.OK,
            registerResponse.StatusCode);

        // Act
        var response = await Client.PostAsJsonAsync(
            LoginUrl, model);

        // Assert
        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);

        var token = await response.Content.ReadAsStringAsync();

        Assert.False(string.IsNullOrWhiteSpace(token));
    }

    [Fact]
    public async Task LoginUserAsync_InvalidPassword_ReturnsBadRequest()
    {
        // Arrange
        var model = NewUser;

        var registerResponse = await Client.PostAsJsonAsync(
            RegisterUrl, model);

        Assert.Equal(
            HttpStatusCode.OK,
            registerResponse.StatusCode);

        var invalidModel = new AuthModel
        {
            Email = model.Email,
            Password = "WrongPassword123!"
        };

        // Act
        var response = await Client.PostAsJsonAsync(
            LoginUrl, invalidModel);

        // Assert
        Assert.Equal(
            HttpStatusCode.BadRequest,
            response.StatusCode);
    }

    [Fact]
    public async Task LoginUserAsync_UserNotFound_ReturnsBadRequest()
    {
        // Arrange
        var model = UnknownUser;

        // Act
        var response = await Client.PostAsJsonAsync(
            LoginUrl, model);

        // Assert
        Assert.Equal(
            HttpStatusCode.BadRequest,
            response.StatusCode);
    }
}
