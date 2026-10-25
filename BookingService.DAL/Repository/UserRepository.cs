using System.Linq.Expressions;
using BookingService.DAL.Context;
using BookingService.DAL.Entity;
using BookingService.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookingService.DAL.Repository;

public class UserRepository : IUserRepository
{
    private BookingContext _context;
    
    public UserRepository(BookingContext context)
    {
        _context = context;
    }
    
    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<bool> ExistsByEmailAsync(string email)
    {
        return await _context.Users
            .AnyAsync(u => u.Email == email);
    }

    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
    }

    public async Task<User?> GetUser(Expression<Func<User, bool>> predicate)
    {
        return await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(predicate);
    }

    public IQueryable<User> GetAllAsQueryable()
    {
        return _context.Users;
    }
}