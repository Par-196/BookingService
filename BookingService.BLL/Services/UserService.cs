using BookingService.BLL.DTO;
using BookingService.BLL.Enums;
using BookingService.BLL.Extensions.Mapping;
using BookingService.BLL.Interfaces;
using BookingService.BLL.Models;
using BookingService.DAL.Context;
using BookingService.DAL.Entity;
using Microsoft.EntityFrameworkCore;

namespace BookingService.BLL.Services;

public class UserService : IUserService
{
    private BookingContext _context;

    public UserService(BookingContext context)
    {
        _context = context;
    }

    public async Task<UserDTO?> GetUserByIdAsync(Guid userId)
    {
        var userModel = await _context.Users.FindAsync(userId);
        
        if(userModel is null)
        {
            return null;
        }

        return userModel.ToDto();
    }
    
    public async Task<List<UserDTO>> GetAllUsersAsync()
    {
        var listUsersDto = await _context.Users
            .Select(u => u.ToDto())
            .ToListAsync();
        
        return listUsersDto;
    }

    public async Task<bool> UpdateUserAsync(UserDTO userDto)
    {
        var userModel = await _context.Users.FindAsync(userDto.Id);
        
        if (userModel is null)
        {
            return false;
        }

        userModel.FirstName = userDto.FirstName;
        userModel.LastName = userDto.LastName;
        userModel.Age = userDto.Age;
        userModel.Email = userDto.Email;
        userModel.Password = userDto.Password;
        userModel.Role = Enum.Parse<DAL.Enums.UserRoles>(userDto.Role.ToString());
        
        var result = await _context.SaveChangesAsync();
        return result > 0;
    }

    public async Task<bool> DeleteUserByIdAsync(Guid userId)
    {
        var usersModel = await _context.Users.FindAsync(userId);

        if (usersModel is null)
        {
            return false;
        }

        _context.Remove(usersModel);
        
        var result = await _context.SaveChangesAsync();
        return result > 0;
    }

    public async Task<UserDTO?> GetUserByEmailAsync(string email)
    {
        var userModel = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
        
        if(userModel is null)
        {
            return null;
        }

        return userModel.ToDto();
    }
}