using BookingService.DAL.Entity;
using BookingService.BLL.Query;

namespace BookingService.DAL.Interfaces;

public interface IBookingRepository
{
    Task<Booking?> GetBookingByIdAsync(Guid id);

    Task<List<Booking>> GetAllBookingsAsync();
    
    Task<List<Booking>?> GetFilteredAndPaginatedBookingsAsync(BookingQuery filter);

    Task<List<Booking>?> GetBookingsByUserIdAsync(Guid userId);

    Task<List<Booking>> GetBookingsByApartmentIdAsync(Guid apartmentId);

    Task<bool> CreateBookingAsync(Booking bookingDto);

    Task<bool> UpdateBookingAsync(Booking bookingDto);

    Task<bool> DeleteBookingByIdAsync(Guid id);
    
    // Task<bool> HasBookingConflictAsync(Guid apartmentId, DateTime startDate, DateTime endDate);

}