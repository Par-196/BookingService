using BookingService.BLL.Interfaces;
using BookingService.BLL.Models;
using BookingService.BLL.Query;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookingService.Controllers;

[ApiController]
[Route("/api/bookings/")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

public class BookingController : ControllerBase
{
    private readonly IBookingService _bookingService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public BookingController(IBookingService bookingService, IHttpContextAccessor IHttpContextAccessor)
    {
        _httpContextAccessor = IHttpContextAccessor;
        _bookingService = bookingService;
    }
    
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetBookingById(Guid id)
    {
        var booking = await _bookingService.GetBookingByIdAsync(id);
        if (booking is null)
        {
            return NotFound("booking not found");
        }
        
        return Ok(booking);  
    }

    [HttpGet]
    public async Task<IActionResult?> GetAllBookingsAsync()
    {
        var listBookings = await _bookingService.GetAllBookingsAsync();

        return Ok(listBookings);
    }
    
    //TODO: add endpoint for getting all bookings but filtered an paginated
    //[FromQuery] BookingFilter filter
    
    // [HttpGet]
    // public async Task<IActionResult> GetFilteredAndPaginatedBookingsAsync(BookingQuery bookingFilter)
    // {
    //     var listBookings = await _bookingService.GetFilteredAndPaginatedBookingsAsync(bookingFilter);
    //
    //     return Ok(listBookings);
    // }
    
    
    [HttpGet("user/{userId:guid}")]
    public async Task<IActionResult> GetBookingsByUserIdAsync(Guid userId)
    {
        
        var listBookings = await _bookingService.GetBookingsByUserIdAsync(userId);
        
        return Ok(listBookings);
    }

    [HttpGet("apartment/{apartmentId:guid}")]
    public async Task<IActionResult> GetBookingsByApartmentIdAsync(Guid apartmentId)
    {
        var listBookings = await _bookingService.GetBookingsByApartmentIdAsync(apartmentId);
        
        return Ok(listBookings);
    }

    [HttpPost]
    public async Task<IActionResult> CreateBookingAsync(BookingDTO bookingDto)
    {
        var result = await _bookingService.CreateBookingAsync(bookingDto);
        if (result)
        {
            return Ok("Booking has been created");
        }

        return BadRequest("Booking creation failed");
    }

    [HttpPatch]
    public async Task<IActionResult> UpdateBookingAsync(BookingDTO bookingDto)
    {
        var result = await _bookingService.UpdateBookingAsync(bookingDto);
        if (result)
        {
            return Ok("Booking has been updated");
        }
        
        return BadRequest("Booking update failed");
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteBookingByIdAsync(Guid id)
    {
        var result = await _bookingService.DeleteBookingByIdAsync(id);
        if (result)
        {
            return Ok("Booking has been deleted");
        }
        
        return BadRequest("Booking creation failed");
    }
}