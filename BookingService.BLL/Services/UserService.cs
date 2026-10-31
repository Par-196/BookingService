
using BookingService.BLL.DTO;
using BookingService.BLL.Extensions.Mapping;
using BookingService.BLL.Interfaces;
using BookingService.BLL.Models;
using BookingService.DAL.Enums;
using BookingService.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookingService.BLL.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;

    public UserService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<UserDTO?> GetUserByIdAsync(Guid userId)
    {
        var user = await _unitOfWork.UserRepository
            .GetUserByIdAsync(userId);

        return user?.ToDto();
    }

    public async Task<List<UserDTO>> GetAllUsersAsync()
    {
        var users = await _unitOfWork.UserRepository
            .GetAllAsync();

        return users.Select(u => u.ToDto()).ToList();
    }

    public async Task<UserDTO?> GetUserByEmailAsync(string email)
    {
        var user = await _unitOfWork.UserRepository
            .GetByEmailAsync(email);

        return user?.ToDto();
    }

    public async Task<bool> UpdateUserAsync(UserDTO userDto)
    {
        var user = await _unitOfWork.UserRepository
            .GetUserByIdAsync(userDto.Id);

        if (user is null)
            return false;

        user.FirstName = userDto.FirstName;
        user.LastName = userDto.LastName;
        user.Age = userDto.Age;
        user.Email = userDto.Email;
        user.Password = userDto.Password;
        user.Role = Enum.Parse<UserRoles>(
            userDto.Role.ToString());

        var updated = await _unitOfWork.UserRepository
            .UpdateAsync(user);

        return updated && await _unitOfWork.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteUserByIdAsync(Guid userId)
    {
        if (userId == Guid.Empty)
            return false;

        var deleted = await _unitOfWork.UserRepository
            .DeleteAsync(userId);

        return deleted && await _unitOfWork.SaveChangesAsync() > 0;
    }
}
