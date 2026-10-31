using System.Linq.Expressions;
using BookingService.DAL.Entity;

namespace BookingService.DAL.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);

    Task<User?> GetUserByIdAsync(Guid id);
    
    Task<List<User>> GetAllAsync();

    Task AddAsync(User user);

    Task<User?> GetUser(Expression<Func<User, bool>> predicate);

    IQueryable<User> GetAllAsQueryable();

    Task<bool> UpdateAsync(User user);

    Task<bool> DeleteAsync(Guid id);
    
    Task<List<User>> GetUsersWithoutPasswordHashAsync();
}