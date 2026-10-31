using System.Net;
using System.Net.Http.Json;
using BookingService.BLL.Extensions.Mapping;
using BookingService.BLL.Models;
using BookingService.DAL.Context;
using BookingService.DAL.Entity;
using BookingService.IntegrationTests.Helpers;
using BookingService.IntegrationTests.Infrastructure;
using BookingService.IntegrationTests.Seeder.TestData;
using DotNet.Testcontainers.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BookingService.IntegrationTests.Controller.Tests;


[Collection("IntegrationTests")]
public class ApartmentControllerIntegrationTest : IntegrationTestBase
{
    private readonly TestUserHelper _userHelper;
    private readonly TestApartmentHelper _apartmentHelper;
    
    public ApartmentControllerIntegrationTest(
        BookingApplicationWebFactory factory) 
        : base(factory)
    {
        _userHelper = new TestUserHelper(factory);
        _apartmentHelper = new TestApartmentHelper(factory);
    }

    [Fact]
    public async Task GetApartmentById_Returns200()
    {
        //Arrange
        var userId = await _userHelper.GetSeededDefaultUserIdAsync();
        Authorize(userId);
        var apartmentId = await _apartmentHelper.GetSeededDefaultApartmentIdAsync();
        
        //Act
        var respond = await Client.GetAsync($"api/apartments/{apartmentId}");

        //Assert
        Assert.Equal(
            HttpStatusCode.OK, 
            respond.StatusCode);
    }
    
    [Fact]
    public async Task GetApartmentById_Returns404()
    {
        //Arrange
        var userId = await _userHelper.GetSeededDefaultUserIdAsync();
        Authorize(userId);
        
        //Act
        var respond = await Client.GetAsync($"api/apartments/{Guid.NewGuid()}");

        //Assert
        Assert.Equal(
            HttpStatusCode.NotFound, 
            respond.StatusCode);
    }
    
    [Fact]
    public async Task GetAllApartmentsAsync_Returns200()
    {
        var userId = await _userHelper.GetSeededDefaultUserIdAsync();
        Authorize(userId);
        //Act
        var respond = await Client.GetAsync($"api/apartments");

        //Assert
        Assert.Equal(
            HttpStatusCode.OK, 
            respond.StatusCode);
    }
    
    [Fact]
    public async Task CreateApartmentAsync_Returns201()
    {
        //Arrange
        var userId = await _userHelper.GetSeededDefaultUserIdAsync();
        Authorize(userId);
        var apartment = _apartmentHelper.GetNewApartment();
        
        //Act
        var respond = await Client.PostAsJsonAsync($"api/apartments/", apartment);
        
        //Assert
        Assert.Equal(
            HttpStatusCode.Created, 
            respond.StatusCode);
    }
    
    [Fact]
    public async Task CreateApartmentAsync_Returns400()
    {
        //Arrange
        var userId = await _userHelper.GetSeededDefaultUserIdAsync();
        Authorize(userId);
        var apartment = _apartmentHelper.GetExistingApartment();
        
        //Act
        var respond = await Client.PostAsJsonAsync($"api/apartments/", new ApartmentDTO());

        //Assert
        Assert.Equal(
            HttpStatusCode.BadRequest, 
            respond.StatusCode);
    }
    
    
    [Fact]
    public async Task UpdateApartmentAsync_Returns200()
    {
        var userId = await _userHelper.GetSeededDefaultUserIdAsync();
        Authorize(userId);
        var apartment = _apartmentHelper.GetExistingApartment();
        apartment.Location = "New Location";
        
        //Act
        var respond = await Client.PatchAsJsonAsync($"api/apartments", apartment);

        //Assert
        Assert.Equal(
            HttpStatusCode.OK, 
            respond.StatusCode);
        
    }
    
    [Fact]
    public async Task UpdateApartmentAsync_Returns400()
    {
        //Arrange
        var userId = await _userHelper.GetSeededDefaultUserIdAsync();
        Authorize(userId);
        
        //Act
        var respond = await Client.PatchAsJsonAsync($"api/apartments", new ApartmentDTO());

        //Assert
        Assert.Equal(
            HttpStatusCode.BadRequest, 
            respond.StatusCode);
    }
    
    [Fact]
    public async Task DeleteApartmentByIdAsync_Returns200()
    {
        var userId = TestUsers.UserToDelete.Id;
        Authorize(userId);
        
        var apartmentId = TestApartments.ApartmentToDelete.Id;
        
        //Act
        var respond = await Client.DeleteAsync( $"/api/apartments/{apartmentId}");

        //Assert
        Assert.Equal(
            HttpStatusCode.OK, 
            respond.StatusCode);

        var scope = Factory.Services.CreateScope();

        var context = scope.ServiceProvider
            .GetRequiredService<BookingContext>();

        var deletedApartment = await context.Apartments
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == apartmentId);

        Assert.Null(deletedApartment);
    }
    
    [Fact]
    public async Task DeleteApartmentByIdAsync_Returns400()
    {
        //Arrange
        var userId = TestUsers.UserToDelete.Id;
        Authorize(userId);

        var apartmentId = TestApartments.ApartmentToDelete.Id;
        
        //Act
        var respond = await Client.DeleteAsync( $"/api/apartments/{Guid.NewGuid()}");

        //Assert
        Assert.Equal(
            HttpStatusCode.BadRequest, 
            respond.StatusCode);

        using var scope = Factory.Services.CreateScope();

        var context = scope.ServiceProvider
            .GetRequiredService<BookingContext>();

        var deletedApartment = await context.Apartments
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == apartmentId);

        Assert.NotNull(deletedApartment);
    }
}