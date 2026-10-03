using BookingService.BLL.Models;

namespace BookingService.BLL.Interfaces;

public interface IAuthorizationService
{ 
    public Task<bool> RegisterUserAsync(UserDTO userDTO);
    
    public Task<string?> LoginUserAsync(UserDTO userDTO);
}