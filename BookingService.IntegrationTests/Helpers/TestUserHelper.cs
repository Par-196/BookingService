using BookingService.DAL.Entity;
using BookingService.DAL.Interfaces;
using BookingService.IntegrationTests.Infrastructure;
using BookingService.IntegrationTests.Seeder.TestData;
using Microsoft.Extensions.DependencyInjection;

namespace BookingService.IntegrationTests.Helpers;


public class TestUserHelper
{
    private readonly BookingApplicationWebFactory _factory;

    public TestUserHelper(BookingApplicationWebFactory factory)
    {
        _factory = factory;
    }

    public async Task<Guid> GetSeededDefaultUserIdAsync()
    {
        using var scope = _factory.Services.CreateScope();

        var userRepository = scope.ServiceProvider
            .GetRequiredService<IUserRepository>();

        var user = await userRepository.GetByEmailAsync(
            TestUsers.DefaultUser.Email);

        if (user is null)
        {
            throw new InvalidOperationException(
                "Test user was not found.");
        }

        return Guid.Parse(user.Id.ToString()!);
    }
    
    public async Task<Guid> GetSeededUserForBookingIdAsync()
    {
        using var scope = _factory.Services.CreateScope();

        var userRepository = scope.ServiceProvider
            .GetRequiredService<IUserRepository>();

        var user = await userRepository.GetByEmailAsync(
            TestUsers.UserForBooking.Email);

        if (user is null)
        {
            throw new InvalidOperationException(
                "Test user was not found.");
        }

        return Guid.Parse(user.Id.ToString()!);
    }
    
    public async Task<User> GetSeededDefaultUserAsync()
    {
        using var scope = _factory.Services.CreateScope();

        var userRepository = scope.ServiceProvider
            .GetRequiredService<IUserRepository>();

        var user = await userRepository.GetByEmailAsync(
            TestUsers.DefaultUser.Email);

        if (user is null)
        {
            throw new InvalidOperationException(
                "Test user was not found.");
        }

        return user;
    }
}
