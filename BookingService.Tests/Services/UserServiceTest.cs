
using BookingService.BLL.Extensions.Mapping;
using BookingService.DAL.Entity;
using BookingService.DAL.Enums;
using BookingService.Tests.Fixtures;
using NSubstitute;

namespace BookingService.Tests.Services;

public class UserServiceTest : IClassFixture<UserServiceFixture>
{
    private readonly UserServiceFixture _fixture;

    private static User CreateTestUser() => new()
    {
        Id = Guid.NewGuid(),
        Email = "test@gmail.com",
        Password = "123123",
        FirstName = "FirstName",
        LastName = "LastName",
        Age = 20,
        Role = UserRoles.Customer
    };

    public UserServiceTest(UserServiceFixture fixture)
    {
        _fixture = fixture;
    }

    // GET USER BY ID

    [Fact]
    public async Task GetUserByIdAsync_RunSuccessfully()
    {
        // Arrange
        var (service, unitOfWork, userRepository) =
            _fixture.Create();

        var user = CreateTestUser();

        userRepository
            .GetUserByIdAsync(user.Id)
            .Returns(user);

        // Act
        var result = await service.GetUserByIdAsync(user.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(user.Id, result.Id);
        Assert.Equal(user.Email, result.Email);

        await userRepository
            .Received(1)
            .GetUserByIdAsync(user.Id);

        await unitOfWork
            .DidNotReceive()
            .SaveChangesAsync();
    }

    [Fact]
    public async Task GetUserByIdAsync_ReturnNull()
    {
        // Arrange
        var (service, unitOfWork, userRepository) =
            _fixture.Create();

        var userId = Guid.NewGuid();

        userRepository
            .GetUserByIdAsync(userId)
            .Returns(Task.FromResult<User?>(null));

        // Act
        var result = await service.GetUserByIdAsync(userId);

        // Assert
        Assert.Null(result);

        await userRepository
            .Received(1)
            .GetUserByIdAsync(userId);

        await unitOfWork
            .DidNotReceive()
            .SaveChangesAsync();
    }

    // GET ALL USERS

    [Fact]
    public async Task GetAllUsersAsync_RunSuccessfully()
    {
        // Arrange
        var (service, unitOfWork, userRepository) =
            _fixture.Create();

        var firstUser = CreateTestUser();

        var secondUser = CreateTestUser();
        secondUser.Email = "second@gmail.com";

        var users = new List<User>
        {
            firstUser,
            secondUser
        };

        userRepository
            .GetAllAsync()
            .Returns(users);

        // Act
        var result = await service.GetAllUsersAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);

        Assert.Contains(result, u => u.Id == firstUser.Id);
        Assert.Contains(result, u => u.Id == secondUser.Id);

        await userRepository
            .Received(1)
            .GetAllAsync();

        await unitOfWork
            .DidNotReceive()
            .SaveChangesAsync();
    }

    [Fact]
    public async Task GetAllUsersAsync_EmptyList_ReturnsEmptyList()
    {
        // Arrange
        var (service, unitOfWork, userRepository) =
            _fixture.Create();

        userRepository
            .GetAllAsync()
            .Returns(new List<User>());

        // Act
        var result = await service.GetAllUsersAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);

        await userRepository
            .Received(1)
            .GetAllAsync();

        await unitOfWork
            .DidNotReceive()
            .SaveChangesAsync();
    }

    [Fact]
    public async Task GetUserByEmailAsync_RunSuccessfully()
    {
        // Arrange
        var (service, unitOfWork, userRepository) =
            _fixture.Create();

        var user = CreateTestUser();

        userRepository
            .GetByEmailAsync(user.Email)
            .Returns(user);

        // Act
        var result = await service.GetUserByEmailAsync(user.Email);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(user.Email, result.Email);
        Assert.Equal(user.Id, result.Id);

        await userRepository
            .Received(1)
            .GetByEmailAsync(user.Email);

        await unitOfWork
            .DidNotReceive()
            .SaveChangesAsync();
    }

    [Fact]
    public async Task GetUserByEmailAsync_ReturnNull()
    {
        // Arrange
        var (service, unitOfWork, userRepository) =
            _fixture.Create();

        var email = "missing@gmail.com";

        userRepository
            .GetByEmailAsync(email)
            .Returns(Task.FromResult<User?>(null));

        // Act
        var result = await service.GetUserByEmailAsync(email);

        // Assert
        Assert.Null(result);

        await userRepository
            .Received(1)
            .GetByEmailAsync(email);
    }

   
    [Fact]
    public async Task UpdateUserAsync_RunSuccessfully()
    {
        // Arrange
        var (service, unitOfWork, userRepository) =
            _fixture.Create();

        var user = CreateTestUser();
        var userDto = user.ToDto();

        userDto.FirstName = "UpdatedName";
        userDto.Email = "updated@gmail.com";

        userRepository
            .GetUserByIdAsync(user.Id)
            .Returns(user);

        userRepository
            .UpdateAsync(Arg.Any<User>())
            .Returns(true);

        unitOfWork
            .SaveChangesAsync()
            .Returns(1);

        // Act
        var result = await service.UpdateUserAsync(userDto);

        // Assert
        Assert.True(result);

        Assert.Equal("UpdatedName", user.FirstName);
        Assert.Equal("updated@gmail.com", user.Email);

        await userRepository
            .Received(1)
            .GetUserByIdAsync(user.Id);

        await userRepository
            .Received(1)
            .UpdateAsync(Arg.Is<User>(u =>
                u.Id == user.Id &&
                u.FirstName == "UpdatedName" &&
                u.Email == "updated@gmail.com"));

        await unitOfWork
            .Received(1)
            .SaveChangesAsync();
    }

    [Fact]
    public async Task UpdateUserAsync_UserNotFound_ReturnsFalse()
    {
        // Arrange
        var (service, unitOfWork, userRepository) =
            _fixture.Create();

        var userDto = CreateTestUser().ToDto();

        userRepository
            .GetUserByIdAsync(userDto.Id)
            .Returns(Task.FromResult<User?>(null));

        // Act
        var result = await service.UpdateUserAsync(userDto);

        // Assert
        Assert.False(result);

        await userRepository
            .Received(1)
            .GetUserByIdAsync(userDto.Id);

        await userRepository
            .DidNotReceive()
            .UpdateAsync(Arg.Any<User>());

        await unitOfWork
            .DidNotReceive()
            .SaveChangesAsync();
    }

    [Fact]
    public async Task UpdateUserAsync_RepositoryReturnsFalse()
    {
        // Arrange
        var (service, unitOfWork, userRepository) =
            _fixture.Create();

        var user = CreateTestUser();

        userRepository
            .GetUserByIdAsync(user.Id)
            .Returns(user);

        userRepository
            .UpdateAsync(Arg.Any<User>())
            .Returns(false);

        // Act
        var result = await service.UpdateUserAsync(user.ToDto());

        // Assert
        Assert.False(result);

        await userRepository
            .Received(1)
            .UpdateAsync(Arg.Any<User>());

        await unitOfWork
            .DidNotReceive()
            .SaveChangesAsync();
    }

    [Fact]
    public async Task UpdateUserAsync_SaveChangesReturnsZero()
    {
        // Arrange
        var (service, unitOfWork, userRepository) =
            _fixture.Create();

        var user = CreateTestUser();

        userRepository
            .GetUserByIdAsync(user.Id)
            .Returns(user);

        userRepository
            .UpdateAsync(Arg.Any<User>())
            .Returns(true);

        unitOfWork
            .SaveChangesAsync()
            .Returns(0);

        // Act
        var result = await service.UpdateUserAsync(user.ToDto());

        // Assert
        Assert.False(result);

        await userRepository
            .Received(1)
            .UpdateAsync(Arg.Any<User>());

        await unitOfWork
            .Received(1)
            .SaveChangesAsync();
    }
    

    [Fact]
    public async Task DeleteUserByIdAsync_RunSuccessfully()
    {
        // Arrange
        var (service, unitOfWork, userRepository) =
            _fixture.Create();

        var userId = Guid.NewGuid();

        userRepository
            .DeleteAsync(userId)
            .Returns(true);

        unitOfWork
            .SaveChangesAsync()
            .Returns(1);

        // Act
        var result = await service.DeleteUserByIdAsync(userId);

        // Assert
        Assert.True(result);

        await userRepository
            .Received(1)
            .DeleteAsync(userId);

        await unitOfWork
            .Received(1)
            .SaveChangesAsync();
    }

    [Fact]
    public async Task DeleteUserByIdAsync_EmptyGuid_ReturnsFalse()
    {
        // Arrange
        var (service, unitOfWork, userRepository) =
            _fixture.Create();

        // Act
        var result = await service.DeleteUserByIdAsync(Guid.Empty);

        // Assert
        Assert.False(result);

        await userRepository
            .DidNotReceive()
            .DeleteAsync(Arg.Any<Guid>());

        await unitOfWork
            .DidNotReceive()
            .SaveChangesAsync();
    }

    [Fact]
    public async Task DeleteUserByIdAsync_RepositoryReturnsFalse()
    {
        // Arrange
        var (service, unitOfWork, userRepository) =
            _fixture.Create();

        var userId = Guid.NewGuid();

        userRepository
            .DeleteAsync(userId)
            .Returns(false);

        // Act
        var result = await service.DeleteUserByIdAsync(userId);

        // Assert
        Assert.False(result);

        await userRepository
            .Received(1)
            .DeleteAsync(userId);

        await unitOfWork
            .DidNotReceive()
            .SaveChangesAsync();
    }

    [Fact]
    public async Task DeleteUserByIdAsync_SaveChangesReturnsZero()
    {
        // Arrange
        var (service, unitOfWork, userRepository) =
            _fixture.Create();

        var userId = Guid.NewGuid();

        userRepository
            .DeleteAsync(userId)
            .Returns(true);

        unitOfWork
            .SaveChangesAsync()
            .Returns(0);

        // Act
        var result = await service.DeleteUserByIdAsync(userId);

        // Assert
        Assert.False(result);

        await userRepository
            .Received(1)
            .DeleteAsync(userId);

        await unitOfWork
            .Received(1)
            .SaveChangesAsync();
    }
}
