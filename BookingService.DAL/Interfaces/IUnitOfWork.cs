namespace BookingService.DAL.Interfaces;

public interface IUnitOfWork
{
    IApartmentRepository ApartmentRepository { get; }
    IBookingRepository BookingRepository { get; }
    IUserRepository UserRepository { get; set; }
    Task<int> SaveChangesAsync();
}