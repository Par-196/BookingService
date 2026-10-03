using BookingService.BLL.Models;

namespace BookingService.BLL.Interfaces;

public interface IUserService
{
    public Task<UserDTO?> GetUserByIdAsync(Guid userId);

    public Task<List<UserDTO>> GetAllUsersAsync();

    public Task<bool> UpdateUserAsync(UserDTO userDto);

    public Task<bool> DeleteUserByIdAsync(Guid userId);

    public Task<UserDTO?> GetUserByEmailAsync(string email);
}