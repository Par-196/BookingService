using System.Net;
using System.Net.Http.Json;
using BookingService.BLL.Extensions.Mapping.Extensions;
using BookingService.BLL.Models;
using BookingService.DAL.Context;
using BookingService.DAL.Entity;
using BookingService.IntegrationTests.Helpers;
using BookingService.IntegrationTests.Infrastructure;
using BookingService.IntegrationTests.Seeder.TestData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.DependencyInjection;

namespace BookingService.IntegrationTests.Controller.Tests;

[Collection("IntegrationTests")]
public class BookingControllerIntegrationTest : IntegrationTestBase
{
    private readonly TestUserHelper _userHelper;
    private readonly TestApartmentHelper _apartmentHelper;
    private readonly TestBookingHelper _bookingHelper;
    
    public BookingControllerIntegrationTest(
        BookingApplicationWebFactory factory) 
        : base(factory)
    {
        _userHelper = new TestUserHelper(factory);
        _apartmentHelper = new TestApartmentHelper(factory);
        _bookingHelper = new TestBookingHelper(factory);
    }

    [Fact]
    public async Task GetBookingById_Returns200()
    {
        //Arrange
        var userId = await _userHelper.GetSeededUserForBookingIdAsync();
        Authorize(userId);
        var bookingId = await _bookingHelper.GetSeededBookingIdAsync();
        
        //Act
        var result = await Client.GetAsync($"/api/bookings/{bookingId}");

        //Assert
        Assert.Equal(
            HttpStatusCode.OK,
            result.StatusCode);
    }

    [Fact]
    public async Task GetBookingById_Returns404()
    {
        //Arrange
        var userId = await _userHelper.GetSeededUserForBookingIdAsync();
        Authorize(userId);
        var booking = await _bookingHelper.GetSeededBookingIdAsync();
        
        //Act
        var result = await Client.GetAsync($"/api/bookings/{Guid.NewGuid()}");

        //Assert
        Assert.Equal(
            HttpStatusCode.NotFound,
            result.StatusCode);
    }
    
    [Fact]
    public async Task GetAllBookingsAsync_Returns200()
    {
        //Arrange
        var userId = await _userHelper.GetSeededUserForBookingIdAsync();
        Authorize(userId);
        
        //Act
        var result = await Client.GetAsync($"/api/bookings/");

        //Assert
        Assert.Equal(
            HttpStatusCode.OK,
            result.StatusCode);
    }
    
    
    [Fact]
    public async Task GetBookingsByUserIdAsync_Returns200()
    {
        //Arrange
        var userId = await _userHelper.GetSeededUserForBookingIdAsync();
        Authorize(userId);
        
        //Act
        var result = await Client.GetAsync($"/api/bookings/user/{userId}");

        //Assert
        Assert.Equal(
            HttpStatusCode.OK,
            result.StatusCode);
    }
    
    [Fact]
    public async Task GetBookingsByApartmentIdAsync_Returns200()
    {
        //Arrange
        var userId = await _userHelper.GetSeededUserForBookingIdAsync();
        Authorize(userId);
        var apartmentId = await _apartmentHelper.GetSeededApartmentForBookingIdAsync();
        
        //Act
        var result = await Client.GetAsync($"/api/bookings/apartment/{apartmentId}");

        //Assert
        Assert.Equal(
            HttpStatusCode.OK,
            result.StatusCode);
    }

   
    [Fact]
    public async Task CreateBookingAsync_Returns200()
    {
        //Arrange
        var userId = await _userHelper.GetSeededUserForBookingIdAsync();
        Authorize(userId);
        var booking = _bookingHelper.GetNewBooking();
        //Act
        var result = await Client.PostAsJsonAsync($"/api/bookings/", booking);

        //Assert
        Assert.Equal(
            HttpStatusCode.OK,
            result.StatusCode);
    }

    [Fact]
    public async Task CreateBookingAsync_Returns404()
    {
        //Arrange
        var userId = await _userHelper.GetSeededUserForBookingIdAsync();
        Authorize(userId);
        
        //Act
        var result = await Client.PostAsJsonAsync<BookingDTO>($"/api/bookings/", null);

        //Assert
        Assert.Equal(
            HttpStatusCode.BadRequest,
            result.StatusCode);
    }
    
    
    
    [Fact]
    public async Task UpdateBookingAsync_Returns200()
    {
        // Arrange
        var userId = await _userHelper.GetSeededUserForBookingIdAsync();
        Authorize(userId);

        var booking = await _bookingHelper.GetSeededDefaultBookingAsync();

        var bookingDto = booking.ToDto();
        bookingDto.EndDate = bookingDto.EndDate.AddDays(2);

        // Act
        using var response = await Client.PatchAsJsonAsync("/api/bookings",bookingDto);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        using var scope = Factory.Services.CreateScope();

        var context = scope.ServiceProvider
            .GetRequiredService<BookingContext>();

        var updatedBooking = await context.Bookings
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.Id == bookingDto.Id);

        Assert.NotNull(updatedBooking);
        Assert.Equal(bookingDto.EndDate, updatedBooking.EndDate);
    }
    
    [Fact]
    public async Task UpdateBookingAsync_Returns400()
    {
        //Arrange
        var userId = await _userHelper.GetSeededUserForBookingIdAsync();
        Authorize(userId);
        
        //Act
        var result = await Client.PatchAsJsonAsync<BookingDTO>($"/api/bookings/", null);

        //Assert
        Assert.Equal(
            HttpStatusCode.BadRequest,
            result.StatusCode);
    }
    
    
    [Fact]
    public async Task DeleteBookingByIdAsync_Returns200()
    {
        // Arrange
        var userId = await _userHelper.GetSeededUserForBookingIdAsync();
        Authorize(userId);

        var bookingId = TestBookings.BookingToDelete.Id;


        // Act
        using var response = await Client.DeleteAsync($"/api/bookings/{bookingId}");

        // Assert
        Assert.Equal(
            HttpStatusCode.OK, 
            response.StatusCode);

        using var scope = Factory.Services.CreateScope();

        var context = scope.ServiceProvider
            .GetRequiredService<BookingContext>();

        var deletedBooking = await context.Bookings
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.Id == bookingId);

        Assert.Null(deletedBooking);
    }
    
    [Fact]
    public async Task DeleteBookingByIdAsync_Returns400()
    {
        //Arrange
        var userId = await _userHelper.GetSeededUserForBookingIdAsync();
        Authorize(userId);
        
        //Act
        var result = await Client.DeleteAsync($"/api/bookings/{Guid.Empty}");

        //Assert
        Assert.Equal(
            HttpStatusCode.BadRequest,
            result.StatusCode);
    }
}