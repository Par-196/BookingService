using BookingService.BLL.DTO;
using BookingService.BLL.Enums;
using BookingService.BLL.Models;
using BookingService.DAL.Entity;

namespace BookingService.BLL.Extensions.Mapping;

public static class UserMappingExtensions
{

        public static UserDTO ToDto(this User user)
        {
                return new UserDTO
                {
                        Id = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Age = user.Age,
                        Email = user.Email,
                        Password = user.Password,
                        Role = Enum.Parse<UserRoles>(user.Role.ToString()),
                };
        }
        
        public static User ToEntity(this UserDTO userDto)
        {
                return new User
                {
                        Id = userDto.Id,
                        FirstName = userDto.FirstName,
                        LastName = userDto.LastName,
                        Age = userDto.Age,
                        Email = userDto.Email,
                        Password = userDto.Password,
                        Role = Enum.Parse<BookingService.DAL.Enums.UserRoles>(userDto.Role.ToString()),
                };
        }

}