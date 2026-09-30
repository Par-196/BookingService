using BookingService.BLL.Interfaces;
using BookingService.BLL.Models;
using BookingService.DAL.Context;
using Microsoft.EntityFrameworkCore;

namespace BookingService.BLL.Services;

public class BookingService : IBookingService
{
    private readonly BookingContext _context;
    
    public BookingService(BookingContext context)
    {
        _context = context;
    }

    public async Task<BookingDTO?> GetBookingById(Guid id)
    {
        var booking = await _context.Bookings.FirstOrDefaultAsync(b => b.Id == id);
        if (booking is null)
        {
            return null;
        }

        return new BookingDTO
        {
            Id = booking.Id,
            StartDate = booking.StartDate,
            EndDate = booking.EndDate,
            ApartmentId = booking.ApartmentId,
            UserId = booking.UserId,
        };
    }
}