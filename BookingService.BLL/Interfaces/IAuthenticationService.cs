using BookingService.BLL.Models;

namespace BookingService.BLL.Interfaces;

public interface IAuthenticationService
{ 
    public Task<bool> RegisterUserAsync(string email, string password);
    
    public Task<string?> LoginUserAsync(string email, string password);
}