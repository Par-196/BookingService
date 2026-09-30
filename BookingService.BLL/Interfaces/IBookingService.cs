using BookingService.BLL.Models;

namespace BookingService.BLL.Interfaces;

public interface IBookingService
{
    Task<BookingDTO?> GetBookingById(Guid id);
    
}