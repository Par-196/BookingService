using BookingService.DAL.Entity;
using BookingService.DAL.Interfaces;
using BookingService.IntegrationTests.Infrastructure;
using BookingService.IntegrationTests.Seeder.TestData;
using Microsoft.Extensions.DependencyInjection;

namespace BookingService.IntegrationTests.Helpers;

public class TestBookingHelper
{
    private readonly BookingApplicationWebFactory _factory;

    public TestBookingHelper(BookingApplicationWebFactory factory)
    {
        _factory = factory;
    }
    
    public async Task<Guid> GetSeededBookingIdAsync()
    {
        using var scope = _factory.Services.CreateScope();

        var bookingRepository = scope.ServiceProvider
            .GetRequiredService<IBookingRepository>();

        var booking = await bookingRepository.GetBookingByIdAsync(
            TestBookings.DefaultBooking.Id);

        if (booking is null)
        {
            throw new InvalidOperationException(
                "Test apartment was not found.");
        }

        return Guid.Parse(booking.Id.ToString()!);
    }
    
    public async Task<Booking> GetSeededDefaultBookingAsync()
    {
        using var scope = _factory.Services.CreateScope();

        var bookingRepository = scope.ServiceProvider
            .GetRequiredService<IBookingRepository>();

        var booking = await bookingRepository.GetBookingByIdAsync(
            TestBookings.DefaultBooking.Id);

        if (booking is null)
        {
            throw new InvalidOperationException(
                "Test apartment was not found.");
        }

        return booking;
    }
    
    public Booking GetNewBooking()
    {
        return new Booking
        {
            Id = Guid.NewGuid(),
            StartDate = TestBookings.DefaultBooking.StartDate,
            EndDate = TestBookings.DefaultBooking.EndDate,
            ApartmentId = TestBookings.DefaultBooking.ApartmentId,
            UserId = TestBookings.DefaultBooking.UserId,
            Apartment = new Apartment 
            {
                Id = Guid.NewGuid(),
                ApartmentType = TestApartments.DefaultApartment.ApartmentType,
                Location = TestApartments.DefaultApartment.Location
            },
            User = new User
            {
                Id = Guid.NewGuid(),
                Email = TestUsers.UserForBooking.Email,
                FirstName = TestUsers.UserForBooking.FirstName,
                Password = TestUsers.UserForBooking.Password,
            }
        };
    }
    
}