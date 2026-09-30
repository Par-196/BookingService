using BookingService.BLL.Interfaces;
using BookingService.BLL.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookingService.Controllers;

[ApiController]
[Route("bookings")]
public class BookingController : ControllerBase
{
    private readonly IBookingService _bookingService;
    
    public BookingController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }
    
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetBookingById(Guid id)
    {
        var booking = await _bookingService.GetBookingById(id);
        if (booking is null)
        {
            return NotFound("booking not found");
        }
        
        return Ok(booking);  
    }
    
    [HttpPost]
    public async Task PostBooking([FromBody] BookingDTO booking)
    {
        
    }
    
    [HttpDelete("{id:guid}")]
    public async Task DeleteBooking(Guid id)
    {
        
    }
}