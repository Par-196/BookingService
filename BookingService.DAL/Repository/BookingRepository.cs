using BookingService.DAL.Context;
using BookingService.DAL.Entity;
using BookingService.DAL.Interfaces;
using BookingService.BLL.Query;
using Microsoft.EntityFrameworkCore;

namespace BookingService.DAL.Repository;

public class BookingRepository : IBookingRepository
{
    private BookingContext _context;
    private IBookingRepository _bookingRepositoryImplementation;

    public BookingRepository(BookingContext context)
    {
        _context = context;
    }

    public async Task<Booking?> GetBookingByIdAsync(Guid id)
    {
        return await _context.Bookings.FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<List<Booking>> GetAllBookingsAsync()
    {
        return await _context.Bookings.ToListAsync();
    }

    public async Task<List<Booking>?> GetFilteredAndPaginatedBookingsAsync(BookingQuery filter)
    {
        var query = _context.Bookings.AsNoTracking();
    
        if (filter.StartDate.HasValue)
        {
            query = query.Where(b =>
                b.StartDate >= filter.StartDate.Value);
        }
        if (filter.EndDate.HasValue)
        {
            query = query.Where(b =>
                b.EndDate <= filter.EndDate.Value);
        }
        if (filter.ApartmentId.HasValue)
        {
            query = query.Where(b =>
                b.ApartmentId == filter.ApartmentId.Value);
        }
    
        var bookings = await query
            .OrderBy(b => b.StartDate)
            .ThenBy(b => b.Id)
            .Skip(filter.Skip)
            .Take(filter.Take)
            .ToListAsync();
        
        return bookings.ToList();
    }

    public async Task<List<Booking>?> GetBookingsByUserIdAsync(Guid userId)
    {
        return await _context.Bookings.Where(b => b.Id == userId).ToListAsync();
    }

    public async Task<List<Booking>> GetBookingsByApartmentIdAsync(Guid apartmentId)
    {
        return await _context.Bookings.Where(b => b.ApartmentId == apartmentId).ToListAsync();
    }

    public async Task<bool> CreateBookingAsync(Booking booking)
    {
        await _context.Bookings.AddAsync(booking);
        return true;
    }

    public async Task<bool> UpdateBookingAsync(Booking booking)
    {
        var bookingModel = await _context.Bookings.FindAsync(booking.Id);
        
        bookingModel.StartDate = booking.StartDate;
        bookingModel.EndDate = booking.EndDate;
        bookingModel.ApartmentId = booking.ApartmentId;
        bookingModel.UserId = booking.UserId;
        return true;
    }

    public async Task<bool> DeleteBookingByIdAsync(Guid id)
    {
        return await _context.Bookings
            .Where(b => b.Id == id)
            .ExecuteDeleteAsync() > 0;
    }

    // public async Task<bool> HasBookingConflictAsync(Guid apartmentId, DateTime startDate, DateTime endDate)
    // {
    //     if (startDate.Day < DateTime.UtcNow.Day || endDate.Day < DateTime.UtcNow.Day || endDate < startDate)
    //     {
    //         return false;
    //     }
    //     
    //     var hasConflict = await _context.Bookings
    //         .AnyAsync(b => b.ApartmentId == apartmentId &&
    //                        startDate < b.EndDate &&
    //                        endDate > b.StartDate
    //         );
    //     
    //     return !hasConflict;
    // }
}