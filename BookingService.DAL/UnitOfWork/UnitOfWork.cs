using BookingService.DAL.Context;
using BookingService.DAL.Interfaces;
using BookingService.DAL.Repository;

namespace BookingService.DAL.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly BookingContext _context;
    public IBookingRepository Bookigns { get; }
    
    public UnitOfWork(BookingContext context)
    {
        _context = context;
        Bookigns = new BookingRepository(_context);
    }
    
    public Task<int> SaveChangesAsync()
    {
        return _context.SaveChangesAsync();
    }

}