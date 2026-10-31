
using BookingService.BLL.Extensions.Mapping;
using BookingService.DAL.Context;
using BookingService.DAL.Entity;
using BookingService.DAL.Enums;
using BookingService.Tests.Fixtures;
using Microsoft.EntityFrameworkCore;
using NSubstitute;

namespace BookingService.Tests.Services;

public class ApartmentServiceTest
    : IClassFixture<ApartmentServiceFixture>
{
    private readonly ApartmentServiceFixture _fixture;

    private static readonly Apartment _apartment = new Apartment
    {
        Id = Guid.NewGuid(),
        ApartmentType = ApartmentTypes.OneBedroom,
        Location = "Germany, Dortmund"
    };

    
    public ApartmentServiceTest(ApartmentServiceFixture fixture)
    {
        _fixture = fixture;
    }

    
    [Fact]
    public async Task GetApartmentByIdAsync_RunSuccessfully()
    {
        // Arrange
        var (service, unitOfWork, apartmentRepository) = 
            _fixture.Create();

        apartmentRepository
            .GetApartmentByIdAsync(_apartment.Id)
            .Returns(_apartment);

        //Act
        var result = await service.GetApartmentByIdAsync(_apartment.Id);
        
        //Assert
        Assert.NotNull(result);
    }
    
    [Fact]
    public async Task GetApartmentByIdAsync_ReturnNull()
    {
        // Arrange
        var (service, unitOfWork, apartmentRepository) = 
            _fixture.Create();

        apartmentRepository
            .GetApartmentByIdAsync(_apartment.Id)
            .Returns(_apartment);

        //Act
        var result = await service.GetApartmentByIdAsync(Guid.NewGuid());
        
        //Assert
        Assert.Null(result);
    }

    
    [Fact]
    public async Task GetAllApartmentsAsync_RunSuccessfully()
    {
        // Arrange
        var (service, unitOfWork, apartmentRepository) = 
            _fixture.Create();

        apartmentRepository
            .GetAllApartmentsAsync()
            .Returns(Task.FromResult((List<Apartment>)[_apartment]));

        //Act
        var result = await service.GetAllApartmentsAsync();
        
        //Assert
        Assert.NotNull(result);
    }
    
    
    [Fact]
    public async Task CreateApartmentAsync_RunSuccessfully()
    {
        // Arrange
        var (service, unitOfWork, apartmentRepository) = 
            _fixture.Create();

        apartmentRepository
            .CreateApartmentAsync(Arg.Any<Apartment>())
            .Returns(true);

        unitOfWork
            .SaveChangesAsync()
            .Returns(1);

        //Act
        var result = await service.CreateApartmentAsync(_apartment.ToDto());
        
        //Assert
        Assert.True(result);

        await apartmentRepository
            .Received(1)
            .CreateApartmentAsync(Arg.Any<Apartment>());

        await unitOfWork
            .Received(1)
            .SaveChangesAsync();
    }
    
    
    [Fact]
    public async Task CreateApartmentAsync_RepositoryReturnsFalse()
    {
        // Arrange
        var (service, unitOfWork, apartmentRepository) =
            _fixture.Create();

        apartmentRepository
            .CreateApartmentAsync(Arg.Any<Apartment>())
            .Returns(false);

        // Act
        var result = await service.CreateApartmentAsync(
            _apartment.ToDto());

        // Assert
        Assert.False(result);

        await apartmentRepository
            .Received(1)
            .CreateApartmentAsync(Arg.Any<Apartment>());
    }

    
    
    [Fact]
    public async Task UpdateApartmentAsync_RunSuccessfully()
    {
        // Arrange
        var (service, unitOfWork, apartmentRepository) = 
            _fixture.Create();

        apartmentRepository
            .UpdateApartmentAsync(Arg.Any<Apartment>())
            .Returns(true);

        unitOfWork
            .SaveChangesAsync()
            .Returns(1);

        //Act
        var result = await service.UpdateApartmentAsync(
            _apartment.ToDto());
        
        //Assert
        Assert.True(result);

        await apartmentRepository
            .Received(1)
            .UpdateApartmentAsync(Arg.Any<Apartment>());

        await unitOfWork
            .Received(1)
            .SaveChangesAsync();
    }
    
    
    [Fact]
    public async Task UpdateApartmentAsync_RepositoryReturnsFalse()
    {
        // Arrange
        var (service, unitOfWork, apartmentRepository) =
            _fixture.Create();
        
        apartmentRepository
            .UpdateApartmentAsync(Arg.Any<Apartment>())
            .Returns(false);
        
        // Act
        var result = await service.UpdateApartmentAsync(
            _apartment.ToDto());
        
        // Assert
        Assert.False(result);
        
        await apartmentRepository
            .Received(1)
            .UpdateApartmentAsync(Arg.Any<Apartment>());
    }
    
    [Fact]
    public async Task DeleteApartmentByIdAsync_RunSuccessfully()
    {
        // Arrange
        var (service, unitOfWork, apartmentRepository) = 
            _fixture.Create();

        apartmentRepository
            .DeleteApartmentByIdAsync(Arg.Any<Guid>())
            .Returns(true);

        unitOfWork
            .SaveChangesAsync()
            .Returns(1);

        //Act
        var result = await service
            .DeleteApartmentByIdAsync(
            _apartment.Id);
        
        //Assert
        Assert.True(result);

        await apartmentRepository
            .Received(1)
            .DeleteApartmentByIdAsync(Arg.Any<Guid>());

        await unitOfWork
            .Received(1)
            .SaveChangesAsync();
    }
    
    
    [Fact]
    public async Task DeleteApartmentByIdAsync_RepositoryReturnsFalse()
    {
        // Arrange
        var (service, unitOfWork, apartmentRepository) =
            _fixture.Create();
        
        apartmentRepository
            .DeleteApartmentByIdAsync(Arg.Any<Guid>())
            .Returns(false);
        
        // Act
        var result = await service.DeleteApartmentByIdAsync(
            _apartment.Id);
        
        // Assert
        Assert.False(result);
        
        await apartmentRepository
            .Received(1)
            .DeleteApartmentByIdAsync(Arg.Any<Guid>());
    }
}