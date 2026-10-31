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

    public async Task<User?> GetUserByIdAsync(Guid id)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Id == id);
    }
    
    public async Task<List<User>> GetAllAsync()
    {
        return await _context.Users
            .AsNoTracking()
            .ToListAsync();
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
    
    public async Task<bool> UpdateAsync(User user)
    {
        var existingUser = await _context.Users.FindAsync(user.Id);

        if (existingUser is null)
            return false;

        existingUser.FirstName = user.FirstName;
        existingUser.LastName = user.LastName;
        existingUser.Age = user.Age;
        existingUser.Email = user.Email;
        existingUser.Password = user.Password;
        existingUser.Role = user.Role;

        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var user = await _context.Users.FindAsync(id);

        if (user is null)
            return false;

        _context.Users.Remove(user);

        return true;
    }

    public async Task<List<User>> GetUsersWithoutPasswordHashAsync()
    {
        return await _context.Users
            .Where(u => u.Password == null!)
            .ToListAsync();
    }
}