using BookingService.DAL.Context;
using BookingService.DAL.Interfaces;
using BookingService.DAL.Repository;

namespace BookingService.DAL.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly BookingContext _context;
    
    public IApartmentRepository ApartmentRepository { get; set; }
    public IBookingRepository BookingRepository { get; set; }
    public IUserRepository UserRepository { get; set; }
    public UnitOfWork(BookingContext context)
    {
        _context = context;
        BookingRepository = new BookingRepository(_context);
        UserRepository = new UserRepository(_context);
        ApartmentRepository = new ApartmentRepository(_context);
    }
    
    public Task<int> SaveChangesAsync()
    {
        return _context.SaveChangesAsync();
    }

}