
using BookingService.DAL.Context;
using BookingService.Tests.Fixtures;
using Microsoft.EntityFrameworkCore;

namespace BookingService.Tests.Services;

public class ApartmentServiceTest
    : IClassFixture<ApartmentServiceFixture>
{
    private readonly ApartmentServiceFixture _fixture;

    public ApartmentServiceTest(ApartmentServiceFixture fixture)
    {
        _fixture = fixture;
    }

    
    [Fact]
    public async Task GetApartmentByIdAsync_RunSuccessfully()
    {
        var (service, unitOfWork, apartmentRepository) = 
            _fixture.Create();
        
        
        
    }




}