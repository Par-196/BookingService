using BookingService.BLL.Models;

namespace BookingService.BLL.Interfaces;

public interface IAuthorizationService
{ 
    public Task<UserDTO> CreateUser(UserDTO user);
}