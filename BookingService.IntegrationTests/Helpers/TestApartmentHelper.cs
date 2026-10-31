using BookingService.DAL.Entity;
using BookingService.DAL.Interfaces;
using BookingService.IntegrationTests.Infrastructure;
using BookingService.IntegrationTests.Seeder.TestData;
using Microsoft.Extensions.DependencyInjection;

namespace BookingService.IntegrationTests.Helpers;

public class TestApartmentHelper
{
    private readonly BookingApplicationWebFactory _factory;

    public TestApartmentHelper(BookingApplicationWebFactory factory)
    {
        _factory = factory;
    }
    
    public async Task<Guid> GetSeededDefaultApartmentIdAsync()
    {
        using var scope = _factory.Services.CreateScope();

        var apartmentRepository = scope.ServiceProvider
            .GetRequiredService<IApartmentRepository>();

        var apartment = await apartmentRepository.GetApartmentByIdAsync(
            TestApartments.DefaultApartment.Id);

        if (apartment is null)
        {
            throw new InvalidOperationException(
                "Test apartment was not found.");
        }

        return Guid.Parse(apartment.Id.ToString()!);
    }
    
    public async Task<Guid> GetSeededApartmentForBookingIdAsync()
    {
        using var scope = _factory.Services.CreateScope();

        var apartmentRepository = scope.ServiceProvider
            .GetRequiredService<IApartmentRepository>();

        var apartment = await apartmentRepository.GetApartmentByIdAsync(
            TestApartments.ApartmentForBooking.Id);

        if (apartment is null)
        {
            throw new InvalidOperationException(
                "Test apartment was not found.");
        }

        return Guid.Parse(apartment.Id.ToString()!);
    }
    
    public Apartment GetExistingApartment()
    {
        return new Apartment
        {
            Id = TestApartments.DefaultApartment.Id,
            ApartmentType = TestApartments.DefaultApartment.ApartmentType,
            Location = TestApartments.DefaultApartment.Location
        };
    }
    
    public Apartment GetNewApartment()
    {
        return new Apartment
        {
            Id = Guid.NewGuid(),
            ApartmentType = TestApartments.DefaultApartment.ApartmentType,
            Location = TestApartments.DefaultApartment.Location
        };
    }
}