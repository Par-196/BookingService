using BookingService.DAL.Context;
using BookingService.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookingService.DAL.Repository;

public class BookingRepository : IBookingRepository
{
    private BookingContext _context;
    
    public BookingRepository(BookingContext context)
    {
        _context = context;
    }
    
    
}