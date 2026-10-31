using System.Net;
using BookingService.DAL.Context;
using BookingService.IntegrationTests.Helpers;
using BookingService.IntegrationTests.Infrastructure;
using BookingService.IntegrationTests.Seeder;
using BookingService.IntegrationTests.Seeder.TestData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BookingService.IntegrationTests.Controller.Tests;

[Collection("IntegrationTests")]
public class UserControllerIntegrationTests : IntegrationTestBase
{
    private readonly TestUserHelper _userHelper;
    
    public UserControllerIntegrationTests(
        BookingApplicationWebFactory factory)
        : base(factory)
    {
        _userHelper = new TestUserHelper(factory);
    }

    [Fact]
    public async Task GetAllUsers_WithoutToken_Returns401()
    {
        // Act
        var response = await Client.GetAsync("/api/users");

        // Assert
        Assert.Equal(
            HttpStatusCode.Unauthorized,
            response.StatusCode);
    }

    [Fact]
    public async Task GetAllUsers_WithToken_Returns200()
    {
        // Arrange
        var userId= await _userHelper.GetSeededDefaultUserIdAsync();
        Authorize(userId);

        // Act
        var response = await Client.GetAsync("/api/users");

        // Assert
        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);
    }

    [Fact]
    public async Task GetUserById_ExistingUser_Returns200()
    {
        // Arrange
        var userId= await _userHelper.GetSeededDefaultUserIdAsync();
        Authorize(userId);

        // Act
        var response = await Client.GetAsync(
            $"/api/users/{userId}");

        // Assert
        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);
    }

    [Fact]
    public async Task GetUserById_NotExisting_Returns400()
    {
        // Arrange
        var userId= await _userHelper.GetSeededDefaultUserIdAsync();
        Authorize(userId);

        var nonExistingId = Guid.NewGuid();

        // Act
        var response = await Client.GetAsync(
            $"/api/users/{nonExistingId}");

        // Assert
        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);
    }

    [Fact]
    public async Task GetUserByEmail_Existing_Returns200()
    {
        // Arrange
        var userId= await _userHelper.GetSeededDefaultUserIdAsync();
        Authorize(userId);

        // Act
        var response = await Client.GetAsync(
            $"/api/users/email/{TestUsers.DefaultUser.Email}");

        // Assert
        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);
    }

    [Fact]
    public async Task GetUserByEmail_NotExisting_Returns400()
    {
        // Arrange
        var userId= await _userHelper.GetSeededDefaultUserIdAsync();
        Authorize(userId);

        // Act
        var response = await Client.GetAsync(
            "/api/users/email/missing@example.com");

        // Assert
        Assert.Equal(
            HttpStatusCode.BadRequest,
            response.StatusCode);
    }

    [Fact]
    public async Task DeleteUser_ExistingUser_Returns200()
    {
        // Arrange
        var userId = TestUsers.UserToDelete.Id;
        Authorize(userId);

        // Act
        var response = await Client.DeleteAsync("/api/users");

        // Assert
        Assert.Equal(
            HttpStatusCode.NoContent,
            response.StatusCode);

        using var scope = Factory.Services.CreateScope();

        var context = scope.ServiceProvider
            .GetRequiredService<BookingContext>();

        var deletedUser = await context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == userId);

        Assert.Null(deletedUser);
    }

}