using System.Linq.Expressions;
using BookingService.DAL.Entity;

namespace BookingService.DAL.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByIdAsync(Guid id);
    Task AddAsync(User user);

    Task<User?> GetUser(Expression<Func<User, bool>> predicate);
    
    IQueryable<User> GetAllAsQueryable();
}