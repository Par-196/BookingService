using System.Linq.Expressions;
using BookingService.DAL.Entity;
using BookingService.Exceptions;
using BookingService.Tests.Fixtures;
using NSubstitute;
using Xunit;

public class AuthenticationServiceTest
    : IClassFixture<AuthenticationServiceFixture>
{
    private const string Email = "test@gmail.com";
    private const string Password = "123123";

    private readonly AuthenticationServiceFixture _fixture;

    public AuthenticationServiceTest(
        AuthenticationServiceFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task LoginUserAsync_RunSuccessfully()
    {
        // Arrange
        var (service, unitOfWork, userRepository) =
            _fixture.Create();

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = Email,
            Password = Password
        };
        
        userRepository
            .GetUser(
                Arg.Any<Expression<Func<User, bool>>>())
            .Returns(Task.FromResult<User?>(user));

        // Act
        var result = await service.LoginUserAsync(
            Email,
            Password
        );

        // Assert
        Assert.NotNull(result);
    }

    [Theory]
    [InlineData("test1@test.com")]
    [InlineData("test2@test.com")]
    public async Task LoginUserAsync_ShouldReturnNull_IfNoUser(
        string email)
    {
        // Arrange
        var (service, unitOfWork, userRepository) =
            _fixture.Create();

        userRepository
            .GetUser(
                Arg.Any<Expression<Func<User, bool>>>())
            .Returns(Task.FromResult<User?>(null));

        // Act
        var result = await service.LoginUserAsync(
            email,
            Password
        );

        // Assert
        Assert.Null(result);
    }
    
    [Fact]
    public async Task RegisterUserAsync_IfUserAlreadyExists()
    {
        // Arrange
        var (service, unitOfWork, userRepository) =
            _fixture.Create();

        var existingUser = new User
        {
            Id = Guid.NewGuid(),
            Email = Email,
            Password = Password
        };

        userRepository
            .GetByEmailAsync(Email)
            .Returns(Task.FromResult<User?>(existingUser));

        // Act & Assert
        await Assert.ThrowsAsync<UserAlreadyExistsException>(
            () => service.RegisterUserAsync(Email, Password)
        );

        await userRepository
            .DidNotReceive()
            .AddAsync(Arg.Any<User>());

        await unitOfWork
            .DidNotReceive()
            .SaveChangesAsync();
    }

    [Theory]
    [InlineData("test1@gmail.com", "123321")]
    [InlineData("test2@gmail.com", "password123")]
    public async Task RegisterUserAsync_RunSuccessfully(
        string email,
        string password)
    {
        // Arrange
        var (service, unitOfWork, userRepository) =
            _fixture.Create();

        userRepository
            .GetByEmailAsync(email)
            .Returns(Task.FromResult<User?>(null));

        // Act
        var result = await service.RegisterUserAsync(
            email,
            password
        );

        // Assert
        Assert.True(result);

        await userRepository
            .Received(1)
            .AddAsync(Arg.Is<User>(u =>
                u.Email == email &&
                u.Password == password &&
                u.Id != Guid.Empty));

        await unitOfWork
            .Received(1)
            .SaveChangesAsync();
    }
}