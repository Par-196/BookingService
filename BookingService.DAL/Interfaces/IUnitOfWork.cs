namespace BookingService.DAL.Interfaces;

public interface IUnitOfWork
{
    IBookingRepository Bookigns { get; }
}