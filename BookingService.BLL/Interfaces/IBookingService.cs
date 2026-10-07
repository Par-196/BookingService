using BookingService.BLL.Models;

namespace BookingService.BLL.Interfaces;

public interface IBookingService
{
    Task<BookingDTO?> GetBookingByIdAsync(Guid id);

    Task<List<BookingDTO>> GetAllBookingsAsync();

    Task<List<BookingDTO>> GetBookingsByUserIdAsync(Guid userId);

    Task<List<BookingDTO>> GetBookingsByApartmentIdAsync(Guid apartmentId);

    Task<bool> CreateBookingAsync(BookingDTO bookingDto);

    Task<bool> UpdateBookingAsync(BookingDTO bookingDto);

    Task<bool> DeleteBookingByIdAsync(Guid id);
}