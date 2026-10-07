using BookingService.BLL.Extensions.Mapping;
using BookingService.BLL.Interfaces;
using BookingService.BLL.Models;
using BookingService.DAL.Context;
using BookingService.DAL.Entity;
using Microsoft.EntityFrameworkCore;
namespace BookingService.BLL.Services;

public class BookingService : IBookingService
{
    private readonly BookingContext _context;
    
    public BookingService(BookingContext context)
    {
        _context = context;
    }

    public async Task<BookingDTO?> GetBookingByIdAsync(Guid id)
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

    public async Task<List<BookingDTO>> GetAllBookingsAsync()
    {
        var listBookingsDto = await _context.Bookings.Select(b => b.ToDto()).ToListAsync();
        
        return listBookingsDto;
    }

    public async Task<List<BookingDTO>> GetBookingsByUserIdAsync(Guid userId)
    {
        var userBookingsDtoList = await _context.Bookings
            .Select(b => b.ToDto())
            .Where(b => b.UserId == userId).ToListAsync();

        return userBookingsDtoList;
    }

    public async Task<List<BookingDTO>> GetBookingsByApartmentIdAsync(Guid apartmentId)
    {
        var apartmentBookingsDtoList = await _context.Bookings
            .Select(b => b.ToDto())
            .Where(b => b.ApartmentId == apartmentId).ToListAsync();

        return apartmentBookingsDtoList;
    }

    public async Task<bool> CreateBookingAsync(BookingDTO bookingDto)
    {
        var bookingModel = bookingDto.ToEntity();

        if (!await IsApartmentAvailableAsync(bookingModel.ApartmentId, bookingModel.StartDate, bookingModel.EndDate))
        {
            return false;
        }
        
        await _context.Bookings.AddAsync(bookingModel);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateBookingAsync(BookingDTO bookingDto)
    {
        var bookingModel = await _context.Bookings.FindAsync(bookingDto.Id);
        
        if (bookingModel is null)
        {
            return false;
        }

        bookingModel.StartDate = bookingDto.StartDate;
        bookingModel.EndDate = bookingDto.EndDate;
        bookingModel.ApartmentId = bookingDto.ApartmentId;
        bookingModel.UserId = bookingDto.UserId;
       
        var result = await _context.SaveChangesAsync();
       
        return result > 0;
    }

    public async Task<bool> DeleteBookingByIdAsync(Guid bookingId)
    {
        var bookingModel = await _context.Bookings.FindAsync(bookingId);
        
        if (bookingModel is null)
        {
            return false;
        }

        _context.Bookings.Remove(bookingModel);
        var result = await _context.SaveChangesAsync();
       
        return result > 0;
    }

    private async Task<bool> IsApartmentAvailableAsync(Guid apartmentId, DateTime startDate, DateTime endDate)
    {
        if (startDate.Day < DateTime.UtcNow.Day || endDate.Day < DateTime.UtcNow.Day || endDate < startDate)
        {
            return false;
        }
        
        var hasConflict = await _context.Bookings
            .AnyAsync(b => b.ApartmentId == apartmentId &&
                           startDate < b.EndDate &&
                           endDate > b.StartDate
                        );
        
        return !hasConflict;
    }
    
    
}